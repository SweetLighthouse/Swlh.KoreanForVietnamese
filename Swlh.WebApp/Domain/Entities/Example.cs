using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Example))] public class Example
{
    public Guid Id { get; set; }

    [Display(Name = "Ví dụ tiếng Hàn")]
    [Required(ErrorMessage = "Ví dụ tiếng Hàn không được để trống.")]
    [MaxLength(1023, ErrorMessage = "Ví dụ tiếng Hàn quá dài.")]
    public string Korean { get; set; } = string.Empty;


    [Display(Name = "Ví dụ tiếng Việt")]
    [Required(ErrorMessage = "Ví dụ tiếng Việt không được để trống.")]
    [MaxLength(1023, ErrorMessage = "Ví dụ tiếng Việt quá dài.")]
    public string Vietnamese { get; set; } = string.Empty;

}
