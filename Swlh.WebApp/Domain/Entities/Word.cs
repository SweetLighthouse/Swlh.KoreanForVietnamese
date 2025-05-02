using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Word))] public class Word
{
    public Guid Id { get; set; }
    [MaxLength(255)] public string WordKr { get; set; } = string.Empty;
    [MaxLength(255)] public string WordVn { get; set; } = string.Empty;
    [MaxLength(255)] public string Type { get; set; } = string.Empty;
    [MaxLength(255)] public string Pronunciation { get; set; } = string.Empty;
    [MaxLength(1023)] public string MeaningKr { get; set; } = string.Empty;
    [MaxLength(1023)] public string MeaningVn { get; set; } = string.Empty;

    //public List<CommentOnKeyword> Comments { get; set; } = [];
}
