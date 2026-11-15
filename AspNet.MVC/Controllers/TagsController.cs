
using AspNet.MVC.Models.Tags;
using AspNet.MVC.Services.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AspNet.MVC.Controllers
{
    [Authorize]
    public class TagsController(
        ITagApiService tagApiService,
        ILogger<TagsController> logger) : Controller
    {
        private readonly ITagApiService _tagApiService = tagApiService;
        private readonly ILogger<TagsController> _logger = logger;

        // All authenticated users can view tags.
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token)
        {
            try
            {
                var tags = await _tagApiService.GetAllAsync(token);

                return View(tags);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load tags.");

                TempData["Error"] = "Unable to connect to the API.";

                return View(new List<TagModel>());
            }
        }

        // ADMIN ONLY
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TagCreateModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            TagCreateModel model,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var response = await _tagApiService.CreateAsync(model, token);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Tag created successfully.";

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
                _logger.LogError(ex, "Unable to create tag.");

                ModelState.AddModelError(
                    string.Empty,
                    "Unable to connect to the API.");

                return View(model);
            }
        }

        // ADMIN ONLY
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken token)
        {
            try
            {
                var tag = await _tagApiService.GetByIdAsync(id, token);

                if (tag == null)
                    return NotFound();

                return View(new TagUpdateModel
                {
                    Name = tag.Name
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load tag {TagId}.", id);

                TempData["Error"] = "Unable to load tag.";

                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            TagUpdateModel model,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var response = await _tagApiService.UpdateAsync(id, model, token);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Tag updated successfully.";

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
                _logger.LogError(ex, "Unable to update tag {TagId}.", id);

                ModelState.AddModelError(
                    string.Empty,
                    "Unable to connect to the API.");

                return View(model);
            }
        }

        // All authenticated users can view tag details.
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            try
            {
                var tag = await _tagApiService.GetByIdAsync(id, token);

                if (tag == null)
                    return NotFound();

                return View(tag);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load tag {TagId}.", id);

                TempData["Error"] = "Unable to load tag.";

                return RedirectToAction(nameof(Index));
            }
        }

        // ADMIN ONLY
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            try
            {
                using var response = await _tagApiService.DeleteAsync(id, token);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Tag deleted successfully.";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["Error"] =
                        "Tag was not found or is currently assigned to products.";
                }
                else
                {
                    TempData["Error"] = "Unable to delete tag.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to delete tag {TagId}.", id);

                TempData["Error"] = "Unable to connect to the API.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
