using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Swlh.Domain.Enums;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;
using System.Threading.Tasks;

namespace Swlh.WebApp.Controllers;

public class HandleReportController(MainDbContext context) : Controller
{
    private bool IsAdmin
    {
        get
        {
            if (!Enum.TryParse(HttpContext.Session.GetString("Role"), out Role role)) return false;
            return role == Role.Admin;
        }
    }

    // xem báo cáo của người dùng
    public async Task<IActionResult> Index()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        List<Report> reports = await context.Reports
            .Include(report => report.Account)
            .ToListAsync();
        return View(reports);
    }


    // hiển thị màn hình xử lý báo cáo
    public IActionResult Handle(Guid? id)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        if(id == null)
        {
            TempData["Msg"] = "Không tìm thấy báo cáo.";
            return RedirectToAction(nameof(Index));
        }
        var report = context.Reports.Include(r => r.Account).FirstOrDefault(r => r.Id == id);
        if (report == null)
        {
            TempData["Msg"] = "Không tìm thấy báo cáo.";
            return RedirectToAction(nameof(Index));
        }
        return View(report);
    }

    // nhận xử lý báo cáo
    [HttpPost]
    public async Task<IActionResult> Handle([FromForm] Report? request)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền thực hiện thao tác này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        if (request == null)
        {
            TempData["Msg"] = "Không tìm thấy form.";
            return RedirectToAction(nameof(Index));
        }
        var report = context.Reports.FirstOrDefault(r => r.Id == request.Id);
        if (report == null)
        {
            TempData["Msg"] = "Không tìm thấy báo cáo.";
            return RedirectToAction(nameof(Index));
        }
        if (string.IsNullOrWhiteSpace(request.Answer))
        {
            TempData["Msg"] = "Vui lòng nhập câu trả lời.";
            return View(report);
        }
        report.Answer = request.Answer;
        report.State = request.State;
        context.Reports.Update(report);
        await context.SaveChangesAsync();

        TempData["Msg"] = "Đã lưu xử lý báo cáo.";
        return RedirectToAction(nameof(Index));
    }
}
