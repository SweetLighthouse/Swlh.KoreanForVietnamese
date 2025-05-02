using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(CommentOnKeywordReaction))] public class CommentOnKeywordReaction
{
    public Guid CommentId { get; set; }
    public CommentOnKeyword Comment { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public bool IsLike { get; set; }
}
