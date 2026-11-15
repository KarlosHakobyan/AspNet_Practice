
using AspNet.MVC.Models.Categories;
using AspNet.MVC.Services.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AspNet.MVC.Controllers
{
    [Authorize]
    public class CategoriesController(
        ICategoryApiService categoryApiService,
        ILogger<CategoriesController> logger) : Controller
    {
        private readonly ICategoryApiService _categoryApiService = categoryApiService;
        private readonly ILogger<CategoriesController> _logger = logger;

        // All authenticated users can view categories.
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token)
        {
            try
            {
                var categories = await _categoryApiService.GetAllAsync(token);

                return View(categories);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load categories.");

                TempData["Error"] = "Unable to connect to the API.";

                return View(new List<CategoryModel>());
            }
        }

        // Admin only.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryCreateModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CategoryCreateModel model,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var response = await _categoryApiService.CreateAsync(model, token);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Category created successfully.";

                    return RedirectToAction(nameof(Index));
                }

                var error = await response.Content.ReadAsStringAsync(token);

                ModelState.AddModelError(
                    string.Empty,
                    string.IsNullOrWhiteSpace(error)
                        ? $"API returned {(int)response.StatusCode}."
                        : error);

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to create category.");

                ModelState.AddModelError(
                    string.Empty,
                    "Unable to connect to the API.");

                return View(model);
            }
        }

        // Admin only.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken token)
        {
            try
            {
                var category = await _categoryApiService.GetByIdAsync(id, token);

                if (category == null)
                    return NotFound();

                return View(new CategoryUpdateModel
                {
                    Name = category.Name
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load category {CategoryId}.", id);

                TempData["Error"] = "Unable to load category.";

                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            CategoryUpdateModel model,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var response = await _categoryApiService.UpdateAsync(id, model, token);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Category updated successfully.";

                    return RedirectToAction(nameof(Index));
                }

                var error = await response.Content.ReadAsStringAsync(token);

                ModelState.AddModelError(
                    string.Empty,
                    string.IsNullOrWhiteSpace(error)
                        ? $"API returned {(int)response.StatusCode}."
                        : error);

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to update category {CategoryId}.", id);

                ModelState.AddModelError(
                    string.Empty,
                    "Unable to connect to the API.");

                return View(model);
            }
        }

        // All authenticated users can view category details.
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            try
            {
                var category = await _categoryApiService.GetByIdAsync(id, token);

                if (category == null)
                    return NotFound();

                return View(category);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load category {CategoryId}.", id);

                TempData["Error"] = "Unable to load category.";

                return RedirectToAction(nameof(Index));
            }
        }

        // Admin only.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            try
            {
                using var response = await _categoryApiService.DeleteAsync(id, token);

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Category deleted successfully.";
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    TempData["Error"] = "Category was not found.";
                else
                    TempData["Error"] = "Unable to delete category.";
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to delete category {CategoryId}.", id);

                TempData["Error"] = "Unable to connect to the API.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
