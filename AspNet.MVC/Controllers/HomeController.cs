using AspNet.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNet.MVC.Controllers;

[Authorize]
public class HomeController(
    ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        if (User.IsInRole("Admin"))
            return RedirectToAction("Index", "Admin");

        if (User.IsInRole("Manager"))
            return RedirectToAction("Index", "Manager");

        if (User.IsInRole("Customer"))
            return RedirectToAction("Index", "Customer");

        return Forbid();
    }

    [AllowAnonymous]
    public IActionResult Privacy() => View();

    [AllowAnonymous]
    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        logger.LogError(
            "An error occurred. TraceIdentifier: {TraceIdentifier}",
            HttpContext.TraceIdentifier);

        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}