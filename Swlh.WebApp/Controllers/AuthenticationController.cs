using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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

    public async Task<IActionResult> MyAccount()
    {
        if(AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Vui lòng đăng nhập để xem thông tin tài khoản.";
            return RedirectToAction(nameof(Login));
        }
        var account = await context.Accounts.Where(acc => acc.Id == AccountId)
            .Include(acc => acc.CommentOnWords)
            .ThenInclude(comment => comment.Keyword)
            .SingleOrDefaultAsync();

        if(account == null)
        {
            TempData["Msg"] = "Tài khoản không tồn tại.";
            return RedirectToAction(nameof(Login));
        }
        return View(account);
    }

    public IActionResult UpdateAccount() => View();

    public IActionResult ChangePassword() => View();
}
