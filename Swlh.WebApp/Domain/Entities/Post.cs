using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Post))] public class Post : BaseEntity
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public Account Account { get; set; } = null!;
    [MaxLength(1023)] public string Title { get; set; } = string.Empty;
    [MaxLength(1023)] public string Description { get; set; } = string.Empty;
    public byte[] Thumbnail { get; set; } = [];
    [MaxLength(65535)] public string Body { get; set; } = string.Empty;
    public int AccessedCount { get; set; }
    public bool IsDisabled { get; set; }

    public List<Tag> Tags { get; set; } = [];
    public List<PostTag> PostTags { get; set; } = [];
    public List<CommentOnPost> Comments { get; set; } = [];

    public List<Account> ReactedAccounts { get; set; } = [];
    public List<PostReaction> Reactions { get; set; } = [];
}
