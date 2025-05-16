using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Keyword))] public class Keyword
{
    [MaxLength(450)]
    public string Key { get; set; } = string.Empty;

    public List<CommentOnKeyword> Comments { get; set; } = [];
    public int SearchCount { get; set; } = 0;
}
