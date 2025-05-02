using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(PostReaction))] public class PostReaction
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!; 
    public bool IsLike { get; set; }
}
