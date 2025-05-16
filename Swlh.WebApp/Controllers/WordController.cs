using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swlh.Domain.Enums;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Controllers;

public class WordController(MainDbContext context) : Controller
{
    private bool IsAdmin
    {
        get
        {
            if (!Enum.TryParse(HttpContext.Session.GetString("Role"), out Role role)) return false;
            return role == Role.Admin;
        }
    }

    // GET: Word
    public async Task<IActionResult> Index()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        return View(await context.Words.ToListAsync());
    }

    // GET: Word/Create
    public IActionResult Create()
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        return View();
    }

    // POST: Word/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,WordKr,WordVn,Type,Pronunciation,MeaningKr,MeaningVn")] Word word)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        if (ModelState.IsValid)
        {
            word.Id = Guid.NewGuid();
            context.Add(word);
            await context.SaveChangesAsync();
            TempData["Msg"] = "Thêm từ vựng thành công.";
            return RedirectToAction(nameof(Index));
        }
        return View(word);
    }

    // GET: Word/Edit/5
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

        var word = await context.Words.FindAsync(id);
        if (word == null)
        {
            return NotFound();
        }
        return View(word);
    }

    // POST: Word/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,WordKr,WordVn,Type,Pronunciation,MeaningKr,MeaningVn")] Word word)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        if (id != word.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(word);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Words.Any(e => e.Id == word.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["Msg"] = "Cập nhật từ vựng thành công.";
            return RedirectToAction(nameof(Index));
        }
        return View(word);
    }

    // GET: Word/Delete/5
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

        var word = await context.Words
            .FirstOrDefaultAsync(m => m.Id == id);
        if (word == null)
        {
            return NotFound();
        }

        return View(word);
    }

    // POST: Word/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!IsAdmin)
        {
            TempData["Msg"] = "Bạn không có quyền truy cập vào trang này.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController)[..^10]);
        }
        var word = await context.Words.FindAsync(id);
        if (word != null)
        {
            context.Words.Remove(word);
        }

        await context.SaveChangesAsync();
        TempData["Msg"] = "Xóa từ vựng thành công.";
        return RedirectToAction(nameof(Index));
    }
}
