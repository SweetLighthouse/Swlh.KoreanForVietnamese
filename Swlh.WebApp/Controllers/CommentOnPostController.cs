using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Swlh.Domain.Enums;
using Swlh.WebApp.Application.Services;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Controllers;

[RequireLogin]
public class CommentOnPostController(MainDbContext context) : Controller
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

    private Guid PostId
    {
        get
        {
            string refererUrl = Request.Headers.Referer.ToString();
            if (string.IsNullOrWhiteSpace(refererUrl)) return Guid.Empty;

            var uri = new Uri(refererUrl);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // Expecting something like: ["Post", "Details", "{guid}"]
            if (segments.Length >= 3 && Guid.TryParse(segments[2], out var guid))
            {
                return guid;
            }

            return Guid.Empty;
        }
    }


    [HttpPost]
    public async Task<IActionResult> Handle([FromForm] string type, [FromForm] Guid commentId, [FromForm] string content)
    {
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Bạn chưa đăng nhập.";
        }
        else
        {
            TempData["Msg"] = type switch
            {
                "add" => await AddComment(content),
                "edit" => await EditComment(commentId, content),
                "delete" => await DeleteComment(commentId),
                _ => "Hành động không rõ."
            };
        }

        string refererUrl = Request.Headers.Referer.ToString();
        return string.IsNullOrWhiteSpace(refererUrl)
                ? RedirectToAction("Index", "Home")
                : Redirect(refererUrl);
    }

    private async Task<string> AddComment(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return "Bình luận không hợp lệ.";
        context.CommentOnPosts.Add(new CommentOnPost
        {
            AccountId = AccountId,
            PostId = PostId,
            Content = content,
        });
        await context.SaveChangesAsync();
        return "Thêm bình luận thành công.";
    }

    private async Task<string> EditComment(Guid commentId, string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return "Bình luận không hợp lệ.";
        var comment = await context.CommentOnPosts.FindAsync(commentId);
        if (comment == null) return "Không tìm thấy bình luận.";
        if (comment.AccountId != AccountId) return "Bạn không có quyền sửa bình luận này.";
        comment.Content = content;
        context.CommentOnPosts.Update(comment);
        await context.SaveChangesAsync();
        return "Sửa bình luận thành công.";
    }

    private async Task<string> DeleteComment(Guid commentId)
    {
        var comment = await context.CommentOnPosts.FindAsync(commentId);
        if (comment == null) return "Không tìm thấy bình luận.";
        var account = await context.Accounts.FindAsync(AccountId);
        if (comment.AccountId != AccountId && account?.Role != Role.Admin) return "Bạn không có quyền xoá bình luận này.";
        context.CommentOnPosts.Remove(comment);
        await context.SaveChangesAsync();
        return "Xoá bình luận thành công.";
    }
}
