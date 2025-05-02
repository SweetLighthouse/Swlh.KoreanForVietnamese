using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Keyword))] public class Keyword
{
    public string Key { get; set; } = string.Empty;

    public List<CommentOnKeyword> Comments { get; set; } = [];
    public int SearchCount { get; set; } = 0;
}
