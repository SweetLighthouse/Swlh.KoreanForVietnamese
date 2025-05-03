using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swlh.Domain.Enums;
using Swlh.WebApp.Application.Dtos.AuthenticationDtos;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Controllers;

public class AuthenticationController(MainDbContext context) : Controller
{
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto, string? backurl)
    {
        if (!ModelState.IsValid) return View(dto);

        if (await context.Accounts.AnyAsync(acc => acc.Username == dto.Username))
        {
            ModelState.AddModelError(nameof(dto.Username), "Tên tài khoản đã tồn tại.");
            return View(dto);
        }

        if (await context.Accounts.AnyAsync(acc => acc.Email == dto.Email))
        {
            ModelState.AddModelError(nameof(dto.Email), "Email đã tồn tại.");
            return View(dto);
        }

        await context.Accounts.AddAsync(new Account
        {
            Username = dto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Email = dto.Email,
            Role = Role.User
        });
        await context.SaveChangesAsync();

        TempData["Msg"] = "Đăng ký thành công. Vui lòng đăng nhập.";
        return RedirectToAction(nameof(Login), new { backurl });
    }

    public IActionResult Login(string? backurl) // get the backurl just fine
    {
        //Uri uri = new("http://x.com" + backurl);
        //var dictionary = QueryHelpers.ParseQuery(uri.Query);
        //if(dictionary.ContainsKey("backurl"))
        //if (uri.Query.FirstOrDefault(q => q.) != null) backurl = uri.Query["backurl"].ToString();
        //ViewBag.BackUrl = backurl ?? "/";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto, string? backurl) // refuse to accept the param
    {
        // the hidden input backurl in the view is for this: 
        // httppost does not work with query params

        if (!ModelState.IsValid) return View(dto);
        var account = await context.Accounts
            .FirstOrDefaultAsync(acc =>
                acc.Username == dto.UsernameOrEmail ||
                acc.Email == dto.UsernameOrEmail
            );

        if (account is null || !BCrypt.Net.BCrypt.Verify(dto.Password, account.Password))
        {
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
            return View(dto);
        }

        if(account.IsDisabled)
        {
            ModelState.AddModelError("", "Tài khoản đã bị khóa.");
            return View(dto);
        }

        HttpContext.Session.SetString("Id", account.Id.ToString());
        HttpContext.Session.SetString("Username", account.Username);
        HttpContext.Session.SetString("Role", account.Role.ToString());

        TempData["Msg"] = "Đăng nhập thành công.";
        return string.IsNullOrWhiteSpace(backurl) 
            || backurl.Contains("/Authentication/Login")
            || backurl.Contains("/Authentication/Register")
            ? RedirectToAction("Index", "Home")
            : Redirect(backurl);
    }

    public IActionResult Logout(string? backurl)
    {
        HttpContext.Session.Clear();

        TempData["Msg"] = "Đăng xuất thành công.";
        return string.IsNullOrEmpty(backurl)
            ? RedirectToAction("Index", "Home")
            : Redirect(backurl);
    }

    private Guid AccountId
    {
        get
        {
            var accountIdRaw = HttpContext.Session.GetString("Id");
            if (accountIdRaw == null || !Guid.TryParse(accountIdRaw, out Guid id)) return Guid.Empty;
            return id;
        }
    }

    public IActionResult MyAccount()
    {
        if(AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Vui lòng đăng nhập để xem thông tin tài khoản.";
            return RedirectToAction(nameof(Login));
        }
        return View("~/Views/Account/Details.cshtml", AccountId);
    }

    public IActionResult UpdateAccount() => View();


    [HttpPost]
    public IActionResult UpdateAccount([FromForm] UpdateAccountDto? dto, string? backurl)
    {
        if (dto == null)
        {
            ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
            return View(dto);
        }
        if (!ModelState.IsValid) return View(dto);

        var currentAccount = context.Accounts.Find(AccountId);
        if (currentAccount == null)
        {
            ModelState.AddModelError("", "Không tìm thấy tài khoản.");
            return View(dto);
        }

        // kiểm tra mật khẩu hiện tại
        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, currentAccount.Password))
        {
            ModelState.AddModelError(nameof(dto.CurrentPassword), "Mật khẩu hiện tại không chính xác.");
            return View(dto);
        }

        // kiểm tra tên tài khoản mới
        if (context.Accounts.Any(acc => acc.Username == dto.Username && acc != currentAccount))
        {
            ModelState.AddModelError(nameof(dto.Username), "Tên tài khoản đã tồn tại.");
            return View(dto);
        }
        else currentAccount.Username = dto.Username;

        // kiểm tra email mới
        if (context.Accounts.Any(acc => acc.Email == dto.Email && acc != currentAccount))
        {
            ModelState.AddModelError(nameof(dto.Email), "Địa chỉ email đã sử dụng.");
            return View(dto);
        }
        else currentAccount.Email = dto.Email;

        context.Entry(currentAccount).State = EntityState.Modified;
        context.SaveChanges();

        TempData["Msg"] = "Cập nhật tài khoản thành công.";
        return string.IsNullOrEmpty(backurl)
            ? RedirectToAction("Index", "Home")
            : Redirect(backurl);
    }



    public IActionResult ChangePassword() => View();


    [HttpPost]
    public IActionResult ChangePassword([FromForm] ChangePasswordDto? dto, string? backurl)
    {
        if (dto == null)
        {
            ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
            return View(dto);
        }
        if (!ModelState.IsValid) return View(dto);

        var currentAccount = context.Accounts.Find(AccountId);
        if (currentAccount == null)
        {
            ModelState.AddModelError("", "Không tìm thấy tài khoản.");
            return View(dto);
        }

        // kiểm tra mật khẩu hiện tại
        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, currentAccount.Password))
        {
            ModelState.AddModelError(nameof(dto.OldPassword), "Mật khẩu hiện tại không chính xác.");
            return View(dto);
        }

        else currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        context.Entry(currentAccount).State = EntityState.Modified;
        context.SaveChanges();

        TempData["Msg"] = "Thay đổi mật khẩu thành công.";
        return string.IsNullOrEmpty(backurl)
            ? RedirectToAction("Index", "Home")
            : Redirect(backurl);
    }
}
