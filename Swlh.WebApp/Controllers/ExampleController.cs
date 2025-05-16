using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swlh.Domain.Enums;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Controllers;

public class ExampleController(MainDbContext context) : Controller
{
    private bool IsAdmin
    {
        get
        {
            if (!Enum.TryParse(HttpContext.Session.GetString("Role"), out Role role)) return false;
            return role == Role.Admin;
        }
    }

    // GET: Examples
    public async Task<IActionResult> Index()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        return View(await context.Examples.ToListAsync());
    }

    // GET: Examples/Create
    public IActionResult Create()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        return View();
    }

    // POST: Examples/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Korean,Vietnamese")] Example example)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        if (ModelState.IsValid)
        {
            example.Id = Guid.NewGuid();
            context.Add(example);
            await context.SaveChangesAsync();
            TempData["Msg"] = "Thêm ví dụ thành công.";
            return RedirectToAction(nameof(Index));
        }
        return View(example);
    }

    // GET: Examples/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        if (id == null)
        {
            return NotFound();
        }

        var example = await context.Examples.FindAsync(id);
        if (example == null)
        {
            return NotFound();
        }
        return View(example);
    }

    // POST: Examples/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Korean,Vietnamese")] Example example)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        if (id != example.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(example);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Examples.Any(e => e.Id == example.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["Msg"] = "Cập nhật ví dụ thành công.";
            return RedirectToAction(nameof(Index));
        }
        return View(example);
    }

    // GET: Examples/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        if (id == null)
        {
            return NotFound();
        }

        var example = await context.Examples
            .FirstOrDefaultAsync(m => m.Id == id);
        if (example == null)
        {
            return NotFound();
        }

        return View(example);
    }

    // POST: Examples/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        var example = await context.Examples.FindAsync(id);
        if (example != null)
        {
            context.Examples.Remove(example);
        }

        await context.SaveChangesAsync();
        TempData["Msg"] = "Xóa ví dụ thành công.";
        return RedirectToAction(nameof(Index));
    }
}
