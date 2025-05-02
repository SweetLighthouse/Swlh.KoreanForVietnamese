using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(CommentOnPostReaction))] public class CommentOnPostReaction
{
    public Guid CommentId { get; set; }
    public CommentOnPost Comment { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public bool IsLike { get; set; }
}
