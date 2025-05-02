using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Swlh.WebApp.Models;

namespace Swlh.WebApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
