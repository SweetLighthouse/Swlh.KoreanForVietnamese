using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Entities.Tag))] public class Tag
{
    public Guid Id { get; set; }
    [MaxLength(255)] public string Value { get; set; } = string.Empty;
    
    
    // Many to Many Post
    public List<Post> Posts { get; set; } = [];
    public List<PostTag> PostTags { get; set; } = [];
}
