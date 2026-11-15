
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.MVC.Controllers;

[Authorize(Roles = "Admin")]
public sealed class AdminController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
