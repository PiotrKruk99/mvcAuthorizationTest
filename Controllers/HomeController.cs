using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvcTest.Models;
using Microsoft.AspNetCore.Authorization;

namespace mvcTest.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("/secrets")]
    [Authorize]
    public IActionResult Secrets()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
