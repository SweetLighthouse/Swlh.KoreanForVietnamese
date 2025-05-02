using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Application.Dtos.AuthenticationDtos;

public class RegisterDto
{
    [Display(Name = "Tên tài khoản")]
    [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
    [MaxLength(60, ErrorMessage = "Tên tài khoản không được dài hơn 60 ký tự.")]
    [MinLength(3, ErrorMessage = "Tên tài khoản không được ngắn hơn 3 ký tự.")] 
    public string Username { get; set; } = null!;


    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MaxLength(60, ErrorMessage = "Mật khẩu không được dài hơn 60 ký tự.")]
    [MinLength(3, ErrorMessage = "Mật khẩu không được ngắn hơn 3 ký tự.")]
    public string Password { get; set; } = null!;


    [Display(Name = "Địa chỉ email")]
    [Required(ErrorMessage = "Địa chỉ email không được để trống.")]
    [MaxLength(60, ErrorMessage = "Địa chỉ email không được dài hơn 60 ký tự.")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")] 
    public string Email { get; set; } = null!;
}
