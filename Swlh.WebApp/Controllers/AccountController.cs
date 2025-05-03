using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swlh.Domain.Enums;
using Swlh.WebApp.Context;

namespace Swlh.WebApp.Controllers;

public class AccountController(MainDbContext context) : Controller
{
    private bool IsAdmin
    {
        get
        {
            if(!Enum.TryParse(HttpContext.Session.GetString("Role"), out Role role)) return false;
            return role == Role.Admin;
        }
    }

    public async Task<IActionResult> Index()
    {
        if(!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        return View(await context.Accounts.ToListAsync());
    }

    public async Task<IActionResult> UpdateRole([FromForm] Guid? accountId, [FromForm] Role? role)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }

        if (accountId == null || role == null) return BadRequest();
        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account == null) return NotFound();
        account.Role = role.Value;
        context.Update(account);
        await context.SaveChangesAsync();
        TempData["Msg"] = $"Đã thay đổi quyền của {account.Username} thành {role}.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> UpdateState([FromForm] Guid? accountId, [FromForm] bool? isDisabled)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }

        if (accountId == null || isDisabled == null) return BadRequest();
        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account == null) return NotFound();
        account.IsDisabled = isDisabled.Value;
        context.Update(account);
        await context.SaveChangesAsync();
        var displayText = account.IsDisabled ? "Bị khóa" : "Bình thường";
        TempData["Msg"] = $"Đã thay đổi trạng thái của {account.Username} thành {displayText}.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Accounts/Details/5
    public IActionResult Details(Guid? id)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        return View(id);
    }
}
