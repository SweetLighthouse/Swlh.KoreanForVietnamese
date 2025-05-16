using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Application.Dtos.ReportDtos;

public class ReportCreateDto
{
    [Display(Name = "Đường dẫn")]
    [Required(ErrorMessage = "Cần dẫn nguồn.")]
    [MaxLength(1023, ErrorMessage = "Link quá dài.")] 
    public string Link { get; set; } = string.Empty;


    [Display(Name = "Mô tả")]
    [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
    [MaxLength(65535, ErrorMessage = "Mô tả quá dài.")]
    public string Description { get; set; } = string.Empty;
}
