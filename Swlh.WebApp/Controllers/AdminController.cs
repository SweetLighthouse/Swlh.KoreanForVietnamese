using Microsoft.AspNetCore.Mvc;
using Swlh.WebApp.Application.Dtos.NavItems;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Swlh.WebApp.Controllers;

public class AdminController(IWebHostEnvironment env) : Controller
{
    public IActionResult NavigationManager() => View();

    [HttpPost("/nav/save")]
    public IActionResult NavigationSaveHandler([FromBody] List<NavItem> navItems)
    {
        var fileName = "nav.json";
        var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "PrivateFiles", fileName);

        var json = JsonSerializer.Serialize(navItems, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });


        System.IO.File.WriteAllText(filePath, json);

        TempData["Msg"] = "Navigation saved successfully.";
        return Ok();
    }


    public IActionResult HomepageManager() => View();

    [HttpPost("/homepage/save")]
    public async Task<IActionResult> HomepageSaveHandler()
    {
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
