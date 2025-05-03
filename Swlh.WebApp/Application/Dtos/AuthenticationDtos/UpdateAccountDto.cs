using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Application.Dtos.AuthenticationDtos;

public class UpdateAccountDto
{
    [Display(Name = "Mật khẩu hiện tại")]
    [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Display(Name = "Username mới")]
    [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
    [MaxLength(60, ErrorMessage = "Tên tài khoản không được dài hơn 60 ký tự.")]
    [MinLength(3, ErrorMessage = "Tên tài khoản không được ngắn hơn 3 ký tự.")]
    [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9._-]*$", ErrorMessage = "Tên tài khoản chỉ được chứa các ký tự a-z, A-Z, 0-9 và phải bắt đầu bằng chữ số.")]
    public string Username { get; set; } = string.Empty;


    [Display(Name = "Địa chỉ email mới")]
    [Required(ErrorMessage = "Địa chỉ email không được để trống.")]
    [MaxLength(60, ErrorMessage = "Địa chỉ email không được dài hơn 60 ký tự.")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
    public string Email { get; set; } = string.Empty;
}
