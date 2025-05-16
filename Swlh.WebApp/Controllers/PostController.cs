using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using Swlh.WebApp.Application.Dtos.BaseDtos;
using Swlh.WebApp.Application.Dtos.PostDtos;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Controllers;

public class PostController(MainDbContext context) : Controller
{
    private Guid AccountId
    {
        get
        {
            var accountIdRaw = HttpContext.Session.GetString("Id");
            if (accountIdRaw == null || !Guid.TryParse(accountIdRaw, out Guid id)) return Guid.Empty;
            return id;
        }
    }

    public async Task<IActionResult> Index([FromQuery] PostSearchDto? dto, [FromQuery] int? page)
    {
        var query = context.Posts
            .OrderByDescending(p => p.UpdatedAt)
            .AsQueryable();

        if(dto != null)
        {
            if (!string.IsNullOrWhiteSpace(dto.PartOfTitle))
            {
                var titleFilter = dto.PartOfTitle.ToLower();
                query = query
                    .Where(p => p.Title.ToLower().Contains(titleFilter));

                TempData[nameof(PostSearchDto.PartOfTitle)] = dto.PartOfTitle;
            }

            dto.TagNames = dto.TagNames.Select(t => t.ToLower()).ToList();

            if (dto.TagNames is not null && dto.TagNames.Count > 0)
            {
                query = query
                    .Where(p => p.Tags.Any(tag => dto.TagNames.Contains(tag.Value.ToLower())));
                
                TempData[nameof(PostSearchDto.TagNames)] = dto.TagNames;
            }       
        }

        if (page == null || page < 1) page = 1;
        int pageSize = 12;
        int totalPosts = await query.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);


        // If page is greater than totalPages, set it to the last page.
        if (page > totalPages && totalPages != 0) page = totalPages;

        query = query
            //.Include(p => p.Account)
            .Include(p => p.Tags)
            .Include(p => p.Reactions)
            .Include(p => p.Comments);

        var pageResultDto = new PageResultDto<Post>
        {
            Page = page.Value,
            PageSize = pageSize,
            TotalItem = totalPosts,
            Items = await query
                .Skip((page.Value - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
        };

        return View(pageResultDto);
    }


    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var post = await context.Posts
            .Include(p => p.Account)
            .Include(p => p.Tags)
            .Include(p => p.Reactions)
            .Include(p => p.Comments)
                .ThenInclude(comment => comment.Account)
            .Include(p => p.Comments)
                .ThenInclude(comment => comment.Reactions)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (post == null) return NotFound();

        post.AccessedCount += 1; 
        context.Update(post); // may be some performance issue?
        context.Entry(post).Property(p => p.AccessedCount).IsModified = true;
        await context.SaveChangesAsync(); // i dont need to await this

        post.Comments = post.Comments.OrderByDescending(c => c.CreatedAt).ToList();

        return View(post);
    }




    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PostRequestDto dto) 
    {
        // required 
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Bạn chưa đăng nhập.";
            return View(dto);
        }
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            ModelState.AddModelError(nameof(PostRequestDto.Title), "Tiêu đề không được để trống.");
            return View(dto);
        }
        if (string.IsNullOrWhiteSpace(dto.Body))
        {
            ModelState.AddModelError(nameof(PostRequestDto.Body), "Nội dung không được để trống.");
            return View(dto);
        }

        using var ms = new MemoryStream();

        if(dto.Thumbnail != null) await dto.Thumbnail.CopyToAsync(ms);

        var post = new Post
        {
            Title = dto.Title,
            Body = dto.Body,
            AccountId = AccountId,
            Thumbnail = ms.ToArray(),
            Description = dto.Description ?? string.Empty
        };

        if (dto.TagNames != null)
        {
            // already existed tags
            List<Tag> existTags = await context.Tags
                .Where(tag => dto.TagNames.Contains(tag.Value)).ToListAsync();

            // make sure EF doesn't try to insert existing tags
            foreach (var tag in existTags) context.Attach(tag);

            // 
            IEnumerable<Tag> newTags = dto.TagNames
                .Except(existTags.Select(tag => tag.Value))
                .Select(s => new Tag { Value = s }); // -> [] (none)

            post.Tags = existTags.Concat(newTags).ToList();
        }

        context.Add(post);
        await context.SaveChangesAsync();
        TempData["Msg"] = "Thêm bài viết thành công.";
        return RedirectToAction(nameof(Details), new { id = post.Id });
    }

    // GET: Post/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();
        var post = await context.Posts
                        .Where(post => post.Id == id)
                        .Include(post => post.Tags)
                        .Include(post => post.PostTags)
                        .SingleOrDefaultAsync();

        if (post == null) return NotFound();

        return View(new PostRequestDto
        {
            Body = post.Body,
            Description = post.Description,
            Id = post.Id,
            //IsDisabled = post.IsDisabled,
            Title = post.Title,
            TagNames = post.Tags.Select(tag => tag.Value).ToList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PostRequestDto dto)
    {
        // required 
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Bạn chưa đăng nhập.";
            return View(dto);
        }
        var post = await context.Posts.Where(p => p.Id == dto.Id).Include(post => post.Tags)
                        .Include(post => post.PostTags).FirstOrDefaultAsync();
        if (post == null)
        {
            TempData["Msg"] = "Không tìm thấy bài viết.";
            return View(dto);
        }
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            ModelState.AddModelError(nameof(PostRequestDto.Title), "Tiêu đề không được để trống.");
            return View(dto);
        }
        else
        {
            post.Title = dto.Title;
        }
        if (string.IsNullOrWhiteSpace(dto.Body))
        {
            ModelState.AddModelError(nameof(PostRequestDto.Body), "Nội dung không được để trống.");
            return View(dto);
        }
        else
        {
            post.Body = dto.Body;
        }

        if (dto.Thumbnail != null && dto.Thumbnail.Length > 0)
        {
            using var ms = new MemoryStream();
            await dto.Thumbnail.CopyToAsync(ms);
            post.Thumbnail = ms.ToArray();
        }
        post.Description = dto.Description ?? "";

        if (dto.TagNames == null)
        {
            ModelState.AddModelError(nameof(PostRequestDto.TagNames), "Cập nhật phải bao gồm các tag, kể cả trống.");
            return View(dto);
        }
        else
        {
            // 1. Fetch existing tags in DB that match the incoming tag names
            List<Tag> existingTags = await context.Tags
                .Where(tag => dto.TagNames.Contains(tag.Value))
                .ToListAsync();

            // 2. Determine which tag names are new
            List<string> existingTagNames = existingTags.Select(t => t.Value).ToList();
            List<string> newTagNames = dto.TagNames.Except(existingTagNames).ToList();

            // 3. Create Tag objects for the new tags
            List<Tag> newTags = newTagNames.Select(name => new Tag { Value = name }).ToList();

            // 4. Add new tags to DB
            context.Tags.AddRange(newTags);
            await context.SaveChangesAsync(); // So they get IDs

            // 5. Combine all valid tags for this post
            List<Tag> allTagsToAssign = existingTags.Concat(newTags).ToList();

            // 6. Remove tags that are no longer selected
            post.Tags.RemoveAll(tag => !dto.TagNames.Contains(tag.Value));

            // 7. Add new tags that are not already in post
            foreach (var tag in allTagsToAssign)
            {
                if (!post.Tags.Any(t => t.Value == tag.Value))
                {
                    post.Tags.Add(tag);
                }
            }

            // Done. EF will track the changes to `post.Tags`
            await context.SaveChangesAsync();

        }


        try
        {
            context.Update(post);
            await context.SaveChangesAsync();
            TempData["Msg"] = "Sửa bài viết thành công.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Posts.Any(e => e.Id == dto.Id))
            {
                TempData["Msg"] = "Không tìm thấy bài viết.";
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Details), new { id = post.Id });
    }

    // GET: Post/Delete/5
    //public async Task<IActionResult> Delete(Guid? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var post = await context.Posts
    //        .Include(p => p.Account)
    //        .FirstOrDefaultAsync(m => m.Id == id);
    //    if (post == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(post);
    //}

    // POST: Post/Delete/5
    //[HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var post = await context.Posts.FindAsync(id);
        if (post != null)
        {
            context.Posts.Remove(post);
        }

        await context.SaveChangesAsync();
        TempData["Msg"] = "Xoá bài viết thành công.";
        return RedirectToAction(nameof(Index));
    }
}
