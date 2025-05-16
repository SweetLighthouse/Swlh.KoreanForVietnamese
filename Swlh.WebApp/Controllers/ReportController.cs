using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swlh.WebApp.Application.Dtos.ReportDtos;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;
using Swlh.WebApp.Domain.Enums;

namespace Swlh.WebApp.Controllers;

public class ReportController(MainDbContext context) : Controller
{
    private Guid AccountId
    {
        get
        {
            var accountIdRaw = HttpContext.Session.GetString("Id");
            if (accountIdRaw == null || !Guid.TryParse(accountIdRaw, out Guid id)) return Guid.Empty;
            return id;
        }
    }

    // người dùng xem báo cáo của mình
    public IActionResult Index()
    {
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Vui lòng đăng nhập để xem báo cáo của bạn.";
            return RedirectToAction(nameof(AuthenticationController.Login), nameof(AuthenticationController)[..^10]);
        }
        var reports = context.Reports.Where(r => r.AccountId == AccountId).ToList();
        return View(reports);
    }

    // người dùng tạo báo cáo
    public IActionResult Create()
    {
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Vui lòng đăng nhập để tạo báo cáo.";
            return RedirectToAction(nameof(AuthenticationController.Login), nameof(AuthenticationController)[..^10]);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ReportCreateDto? dto, [FromForm] string? backurl)
    {
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Vui lòng đăng nhập để tạo báo cáo.";
            return RedirectToAction(nameof(AuthenticationController.Login), nameof(AuthenticationController)[..^10]);
        }
        if (dto == null)
        {
            ModelState.AddModelError("", "Không nhận được dữ liệu.");
            return View(dto);
        }
        if (!ModelState.IsValid) return View(dto);
        context.Reports.Add(new Report()
        {
            AccountId = AccountId,
            Link = dto.Link,
            Description = dto.Description,
            State = ReportStatus.Pending,
            Answer = null,
        });
        await context.SaveChangesAsync();
        TempData["Msg"] = "Tạo báo cáo thành công.";
        return string.IsNullOrWhiteSpace(backurl)
            ? RedirectToAction(nameof(Index))
            : Redirect(backurl);
    }

    // người dùng sửa báo cáo của mình
    public IActionResult Edit(Guid id)
    {
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Vui lòng đăng nhập để sửa báo cáo.";
            return RedirectToAction(nameof(AuthenticationController.Login), nameof(AuthenticationController)[..^10]);
        }
        var report = context.Reports.Find(id);
        if (report == null)
        {
            TempData["Msg"] = "Không tìm thấy báo cáo.";
            return RedirectToAction(nameof(Index));
        }
        return View(new ReportEditDto
        {
            Id = report.Id,
            Link = report.Link,
            Description = report.Description,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] ReportEditDto? dto, [FromForm] string? backurl)
    {
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Vui lòng đăng nhập để sửa báo cáo.";
            return RedirectToAction(nameof(AuthenticationController.Login), nameof(AuthenticationController)[..^10]);
        }
        if (dto == null)
        {
            ModelState.AddModelError("", "Không nhận được dữ liệu.");
            return View(dto);
        }
        if (!ModelState.IsValid) return View(dto);
        var report = context.Reports.Find(dto.Id);
        if (report == null)
        {
            ModelState.AddModelError("", "Không tìm thấy báo cáo.");
            return View(dto);
        }
        if (report.AccountId != AccountId)
        {
            TempData["Msg"] = "Bạn không có quyền xóa báo cáo này.";
            return RedirectToAction(nameof(Index));
        }
        if (report.State != ReportStatus.Pending)
        {
            TempData["Msg"] = "Báo cáo đã được giải quyết, không thể sửa.";
            return RedirectToAction(nameof(Index));
        }
        report.Link = dto.Link;
        report.Description = dto.Description;
        context.Entry(report).State = EntityState.Modified;
        await context.SaveChangesAsync();
        TempData["Msg"] = "Sửa báo cáo thành công.";
        return string.IsNullOrWhiteSpace(backurl)
            ? RedirectToAction(nameof(Index))
            : Redirect(backurl);
    }

    // người dùng xóa báo cáo của mình
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (AccountId == Guid.Empty)
        {
            TempData["Msg"] = "Vui lòng đăng nhập để xóa báo cáo.";
            return RedirectToAction(nameof(AuthenticationController.Login), nameof(AuthenticationController)[..^10]);
        }
        var report = context.Reports.Find(id);
        if (report == null)
        {
            TempData["Msg"] = "Không tìm thấy báo cáo.";
            return RedirectToAction(nameof(Index));
        }
        if(report.AccountId != AccountId)
        {
            TempData["Msg"] = "Bạn không có quyền xóa báo cáo này.";
            return RedirectToAction(nameof(Index));
        }
        if (report.State != ReportStatus.Pending)
        {
            TempData["Msg"] = "Báo cáo đã được giải quyết, không thể xoá.";
            return RedirectToAction(nameof(Index));
        }
        context.Reports.Remove(report);
        await context.SaveChangesAsync();
        TempData["Msg"] = "Xóa báo cáo thành công.";
        return RedirectToAction(nameof(Index));
    }
}
