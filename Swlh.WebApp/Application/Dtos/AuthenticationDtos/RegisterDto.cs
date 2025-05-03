using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Application.Dtos.AuthenticationDtos;

public class RegisterDto
{
    [Display(Name = "Tên tài khoản")]
    [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
    [MaxLength(60, ErrorMessage = "Tên tài khoản không được dài hơn 60 ký tự.")]
    [MinLength(3, ErrorMessage = "Tên tài khoản không được ngắn hơn 3 ký tự.")]
    [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9._-]*$", ErrorMessage = "Tên tài khoản chỉ được chứa các ký tự a-z, A-Z, 0-9 và phải bắt đầu bằng chữ số.")]

    public string Username { get; set; } = null!;


    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MaxLength(60, ErrorMessage = "Mật khẩu không được dài hơn 60 ký tự.")]
    [MinLength(3, ErrorMessage = "Mật khẩu không được ngắn hơn 3 ký tự.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[+=\-~`[\]{}\\|""':;.,<>/?()_!@#$%^&*]).+$", ErrorMessage = "Mật khẩu yếu. Cần ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]

    public string Password { get; set; } = null!;


    [Display(Name = "Địa chỉ email")]
    [Required(ErrorMessage = "Địa chỉ email không được để trống.")]
    [MaxLength(60, ErrorMessage = "Địa chỉ email không được dài hơn 60 ký tự.")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")] 
    public string Email { get; set; } = null!;
}
