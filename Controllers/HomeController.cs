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

    [Route("/moresecrets")]
    [Authorize(Roles = "admin")]
    public IActionResult MoreSecrets()
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
    public async Task<IActionResult> Login(string login = "", string password = "", string returnUrl = "/")
    {
        if ((login.Equals("aaa") && password.Equals("aaa"))
            || (login.Equals("bbb") && password.Equals("bbb")))
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, login ?? ""));
            claims.Add(new Claim("login", login ?? ""));

            if ((login ?? "").Equals("aaa"))
                claims.Add(new Claim(ClaimTypes.Role, "admin"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);

            //return Redirect(((returnUrl ?? "/").Equals(string.Empty) ? "/" : returnUrl) ?? "/");
            return Redirect(returnUrl);
        }
        else
        {
            ViewBag.returnUrl = returnUrl ?? string.Empty;
            ViewBag.returnError = "wrong user name or password";
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

    [Route("/illegal")]
    public IActionResult Illegal()
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
