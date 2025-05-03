using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Application.Dtos.AuthenticationDtos;

public class ChangePasswordDto
{
    [Display(Name = "Mật khẩu cũ")]
    [Required(ErrorMessage = "Mật khẩu cũ không được để trống.")]
    [MaxLength(60, ErrorMessage = "Mật khẩu cũ không được dài hơn 60 ký tự.")]
    [MinLength(3, ErrorMessage = "Mật khẩu không được ngắn hơn 3 ký tự.")]
    public string OldPassword { get; set; } = null!;

    [Display(Name = "Mật khẩu mới")]
    [Required(ErrorMessage = "Mật khẩu mới không được để trống.")]
    [MaxLength(60, ErrorMessage = "Mật khẩu mới không được dài hơn 60 ký tự.")]
    [MinLength(3, ErrorMessage = "Mật khẩu mới không được ngắn hơn 3 ký tự.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).+$", ErrorMessage = "Mật khẩu yếu. Cần ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
    public string NewPassword { get; set; } = null!;

    [Display(Name = "Xác nhận mật khẩu mới")]
    [Required(ErrorMessage = "Vui lòng xác nhận lại mật khẩu mới.")]
    [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string ConfirmNewPassword { get; set; } = null!;
}
