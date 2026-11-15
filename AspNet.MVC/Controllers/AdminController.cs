
using AspNet.MVC.Models.Dashboard;
using AspNet.MVC.Services.Api;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.MVC.Controllers;

[Authorize(Roles = "Admin")]
public sealed class AdminController(
    IProductApiService productApiService,
    ICategoryApiService categoryApiService,
    ITagApiService tagApiService,
    ILogger<AdminController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(
        CancellationToken token)
    {
        var model = new AdminDashboardViewModel();

        try
        {
            var productsTask = productApiService.GetAllAsync(token);
            var categoriesTask = categoryApiService.GetAllAsync(token);
            var tagsTask = tagApiService.GetAllAsync(token);

            await Task.WhenAll(
                productsTask,
                categoriesTask,
                tagsTask);

            var products = await productsTask;
            var categories = await categoriesTask;
            var tags = await tagsTask;

            model.TotalProducts = products.Count;
            model.TotalCategories = categories.Count;
            model.TotalTags = tags.Count;

            model.IsSystemHealthy = true;
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unable to load Admin Dashboard statistics.");

            model.IsSystemHealthy = false;
        }

        return View(model);
    }
}
