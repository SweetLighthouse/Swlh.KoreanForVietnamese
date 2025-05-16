using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Application.Dtos.AuthenticationDtos;

public class ForgotPasswordDto
{
    [Display(Name = "Tên tài khoản hoặc email")]
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập hoặc email.")]
    public string UsernameOrEmail { get; set; } = null!;
}
