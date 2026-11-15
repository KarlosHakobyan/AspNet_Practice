
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.MVC.Controllers;

[Authorize(Roles = "Manager")]
public sealed class ManagerController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
