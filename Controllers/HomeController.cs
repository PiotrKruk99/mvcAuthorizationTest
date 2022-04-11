using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvcTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

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

    [HttpGet("/login")]
    public IActionResult Login(string returnUrl)
    {
        ViewBag.returnUrl = returnUrl ?? string.Empty;
        return View();
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login(string login, string password, string returnUrl)
    {
        if (login.Equals("aaa") && password.Equals("bbb"))
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("login", login));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);

            // if ((returnUrl ?? string.Empty).Equals(string.Empty))
            //     return Redirect("/");
            // else
            //     return Redirect(returnUrl);
            return Redirect(((returnUrl ?? "/").Equals(string.Empty) ? "/" : returnUrl) ?? "/"); //nadmiarowy kod dla wyeliminowania ostrzeżenia
        }
        else
        {
            ViewBag.returnUrl = returnUrl ?? string.Empty;
            return View();
        }
    }

    [Route("/logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
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
