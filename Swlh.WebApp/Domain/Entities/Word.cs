using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Word))] public class Word
{
    public Guid Id { get; set; }

    [Display(Name = "Từ vựng tiếng Hàn")]
    [Required(ErrorMessage = "Từ vựng tiếng Hàn không được để trống.")]
    [MaxLength(255, ErrorMessage = "Từ vựng tiếng Hàn quá dài.")] 
    public string WordKr { get; set; } = string.Empty;


    [Display(Name = "Từ vựng tiếng Việt")]
    [Required(ErrorMessage = "Từ vựng tiếng Việt không được để trống.")]
    [MaxLength(255, ErrorMessage = "Từ vựng tiếng Việt quá dài.")] 
    public string WordVn { get; set; } = string.Empty;


    [Display(Name = "Loại từ")]
    [Required(ErrorMessage = "Loại từ không được để trống.")]
    [MaxLength(255, ErrorMessage = "Loại từ quá dài.")] 
    public string Type { get; set; } = string.Empty;


    [Display(Name = "Phát âm")]
    [Required(ErrorMessage = "Phát âm không được để trống.")]
    [MaxLength(255, ErrorMessage = "Phát âm quá dài.")] 
    public string Pronunciation { get; set; } = string.Empty;


    [Display(Name = "Nghĩa tiếng Hàn")]
    [Required(ErrorMessage = "Nghĩa tiếng Hàn không được để trống.")]
    [MaxLength(1023, ErrorMessage = "Nghĩa tiếng Hàn quá dài.")] 
    public string MeaningKr { get; set; } = string.Empty;


    [Display(Name = "Nghĩa tiếng Việt")]
    [Required(ErrorMessage = "Nghĩa tiếng Việt không được để trống.")]
    [MaxLength(1023, ErrorMessage = "Nghĩa tiếng Việt quá dài.")]
    public string MeaningVn { get; set; } = string.Empty;
}
