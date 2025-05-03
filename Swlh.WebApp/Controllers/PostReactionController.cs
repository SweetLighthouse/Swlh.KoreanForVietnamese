using Microsoft.AspNetCore.Mvc;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostReactionController(MainDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> HandleReact(Guid commentId, bool isLike)
    {
        // commentId is actually a postId
        // for the sake of minimun edit, i keep it that way

        var accountIdRaw = HttpContext.Session.GetString("Id");
        if (!Guid.TryParse(accountIdRaw, out var accountId)) return Unauthorized();
        if (!context.Posts.Any(x => x.Id == commentId)) return NotFound();
        var reaction = context.PostReactions.SingleOrDefault(r => r.AccountId == accountId && r.PostId == commentId);
        if (reaction == null)
        {
            context.PostReactions.Add(new PostReaction
            {
                AccountId = accountId,
                PostId = commentId,
                IsLike = isLike
            });
            await context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            if (reaction.IsLike == isLike) // already liked/disliked, but still like/dislike -> delete
            {
                context.PostReactions.Remove(reaction);
                await context.SaveChangesAsync();
                return Ok();
            }
            else // just a switch
            {
                reaction.IsLike = isLike;
                context.Update(reaction);
                await context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}
