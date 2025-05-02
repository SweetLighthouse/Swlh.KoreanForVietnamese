using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Application.Dtos.AuthenticationDtos;

public class LoginDto
{
    [Display(Name = "Tên tài khoản hoặc email")]
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập hoặc email.")]
    public string UsernameOrEmail { get; set; } = null!;



    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    public string Password { get; set; } = null!;
}
