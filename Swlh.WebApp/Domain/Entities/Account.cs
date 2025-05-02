using Swlh.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Account))] public class Account : BaseEntity
{
    public Guid Id { get; set; }
    [MaxLength(60)] public required string Username { get; set; }
    [MaxLength(60)] public required string Password { get; set; }
    [MaxLength(60)] public required string Email { get; set; } = null!;
    public required Role Role { get; set; }
    public bool IsDisabled { get; set; }

    
    public List<CommentOnKeyword> CommentOnWords { get; set; } = [];
    public List<CommentOnKeyword> CommentOnWordsReacted { get; set; } = [];
    public List<CommentOnKeywordReaction> CommentOnWordReactions { get; set; } = [];



    public List<Post> Posts { get; set; } = [];
    public List<Post> PostsReacted { get; set; } = [];
    public List<PostReaction> PostReactions { get; set; } = [];


    public List<CommentOnPost> CommentOnPosts { get; set; } = [];
    public List<CommentOnPost> CommentOnPostsReacted { get; set; } = [];
    public List<CommentOnPostReaction> CommentOnPostReactions { get; set; } = [];


    public List<Report> ReportsCreated { get; set; } = [];
}
