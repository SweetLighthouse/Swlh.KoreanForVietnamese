using Microsoft.AspNetCore.Mvc;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentOnPostReactionController(MainDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> HandleReact(Guid commentId, bool isLike)
    {
        var accountIdRaw = HttpContext.Session.GetString("Id");
        if (!Guid.TryParse(accountIdRaw, out var accountId)) return Unauthorized();
        if (!context.CommentOnPosts.Any(x => x.Id == commentId)) return NotFound();
        var reaction = context.CommentOnPostReactions.SingleOrDefault(r => r.AccountId == accountId && r.CommentId == commentId);
        if (reaction == null)
        {
            context.CommentOnPostReactions.Add(new CommentOnPostReaction
            {
                AccountId = accountId,
                CommentId = commentId,
                IsLike = isLike
            });
            await context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            if (reaction.IsLike == isLike) // already liked/disliked, but still like/dislike -> delete
            {
                context.CommentOnPostReactions.Remove(reaction);
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