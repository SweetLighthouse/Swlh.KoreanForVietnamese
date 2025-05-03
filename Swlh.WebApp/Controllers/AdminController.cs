using Microsoft.AspNetCore.Mvc;
using Swlh.Domain.Enums;
using Swlh.WebApp.Application.Dtos.NavItems;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Swlh.WebApp.Controllers;

public class AdminController(IWebHostEnvironment env) : Controller
{
    private bool IsAdmin
    {
        get
        {
            if (!Enum.TryParse(HttpContext.Session.GetString("Role"), out Role role)) return false;
            return role == Role.Admin;
        }
    }

    public IActionResult NavigationManager()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }

        return View();
    }

    [HttpPost("/nav/save")]
    public IActionResult NavigationSaveHandler([FromBody] List<NavItem> navItems)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }

        var fileName = "nav.json";
        var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "PrivateFiles", fileName);

        var json = JsonSerializer.Serialize(navItems, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });


        System.IO.File.WriteAllText(filePath, json);

        TempData["Msg"] = "Lưu thành công.";
        return Ok();
    }



    public IActionResult FooterManager()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }

        return View();
    }

    [HttpPost("/footer/save")]
    public IActionResult FooterSaveHandler([FromBody] List<NavItem> navItems)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }

        var fileName = "footer.json";
        var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "PrivateFiles", fileName);

        var json = JsonSerializer.Serialize(navItems, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });


        System.IO.File.WriteAllText(filePath, json);

        TempData["Msg"] = "Lưu thành công.";
        return Ok();
    }



    public IActionResult HomepageManager()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }

        return View();
    }

    [HttpPost("/homepage/save")]
    public async Task<IActionResult> HomepageSaveHandler()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }

        using var reader = new StreamReader(Request.Body);
        string html = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(html)) return BadRequest("Empty content.");

        var fileName = "_Homepage.json";
        var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "PrivateFiles", fileName);

        try
        {
            await System.IO.File.WriteAllTextAsync(filePath, html);
            TempData["Msg"] = "Lưu thay đổi thành công.";
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Lỗi khi lưu file: " + ex.Message);
        }
    }
}
