
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.MVC.Controllers;

[Authorize(Roles = "Customer")]
public sealed class CustomerController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
