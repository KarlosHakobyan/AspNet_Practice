using AspNet.MVC.Models.Products;
using AspNet.MVC.Services.Api;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AspNet.MVC.Controllers
{
    public class ProductsController(
        IProductApiService productApiService,
        ILogger<ProductsController> logger) : Controller
    {
        private readonly IProductApiService _productApiService =
            productApiService;

        private readonly ILogger<ProductsController> _logger = logger;

        // GET: /Products
        public async Task<IActionResult> Index(CancellationToken token)
        {
            try
            {
                var products = await _productApiService.GetAllAsync(token);

                return View(products);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load products from API.");

                TempData["Error"] = "Unable to connect to the API.";

                return View(new List<ProductModel>());
            }
        }

        // GET: /Products/Details/5
        public async Task<IActionResult> Details(int id,CancellationToken token)
        {
            try
            {
                var product = await _productApiService.GetByIdAsync(id, token);

                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load product {ProductId}.", id);

                TempData["Error"] = "Unable to load product details.";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Products/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProductCreateModel());
        }

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateModel model,CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var response = await _productApiService.CreateAsync(model,token);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Product created successfully.";

                    return RedirectToAction(nameof(Index));
                }

                var error = await response.Content.ReadAsStringAsync(token);

                ModelState.AddModelError(string.Empty,string.IsNullOrWhiteSpace(error)
                        ? $"API returned {(int)response.StatusCode}."
                        : error);

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to create product.");

                ModelState.AddModelError(string.Empty,"Unable to connect to the API.");

                return View(model);
            }
        }

        // GET: /Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken token)
        {
            try
            {
                var product = await _productApiService.GetByIdAsync(id, token);

                if (product == null)
                    return NotFound();

                var model = new ProductUpdateModel
                {
                    Name = product.Name,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                };

                ViewBag.ProductId = id;

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to load product {ProductId}.", id);

                TempData["Error"] = "Unable to load product for editing.";

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ProductUpdateModel model,CancellationToken token)
        {
            ViewBag.ProductId = id;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var response = await _productApiService.UpdateAsync(id,model,token);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Product updated successfully.";

                    return RedirectToAction(nameof(Index));
                }

                var error = await response.Content.ReadAsStringAsync(token);

                ModelState.AddModelError(string.Empty,string.IsNullOrWhiteSpace(error)
                        ? $"API returned {(int)response.StatusCode}."
                        : error);

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to update product {ProductId}.", id);

                ModelState.AddModelError(string.Empty,"Unable to connect to the API.");

                return View(model);
            }
        }

        // POST: /Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id,CancellationToken token)
        {
            try
            {
                using var response = await _productApiService.DeleteAsync(id,token);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Product deleted successfully.";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["Error"] = "Product was not found.";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync(token);

                    TempData["Error"] = string.IsNullOrWhiteSpace(error)
                        ? $"Unable to delete product. API returned {(int)response.StatusCode}."
                        : error;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to delete product {ProductId}.", id);

                TempData["Error"] = "Unable to connect to the API.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}