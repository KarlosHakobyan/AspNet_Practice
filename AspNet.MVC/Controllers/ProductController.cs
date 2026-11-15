
using AspNet.MVC.Models.Products;
using AspNet.MVC.Models.ProductDetails;
using AspNet.MVC.Services.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AspNet.MVC.Controllers;

[Authorize]
public class ProductsController(
    IProductApiService productApiService,
    IProductDetailsApiService productDetailsApiService,
    ILogger<ProductsController> logger) : Controller
{
    private readonly IProductApiService _productApiService =
        productApiService;

    private readonly IProductDetailsApiService _productDetailsApiService =
        productDetailsApiService;

    private readonly ILogger<ProductsController> _logger = logger;

    // GET: /Products
    [HttpGet]
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
    [HttpGet]
    public async Task<IActionResult> Details(
        int id,
        CancellationToken token)
    {
        try
        {
            var product = await _productApiService.GetByIdAsync(id, token);

            if (product == null)
                return NotFound();

            var details = await _productDetailsApiService.GetByIdAsync(
                id, token);

            var model = new ProductDetailsPageModel
            {
                Product = product,
                Details = details
            };

            return View(model);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unable to load product {ProductId}.", id);

            TempData["Error"] = "Unable to load product details.";

            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Products/Create
    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new ProductCreateModel());
    }

    // POST: /Products/Create
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ProductCreateModel model,
        CancellationToken token)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            using var response = await _productApiService.CreateAsync(
                model, token);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Product created successfully.";

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
            _logger.LogError(ex, "Unable to create product.");

            ModelState.AddModelError(
                string.Empty,
                "Unable to connect to the API.");

            return View(model);
        }
    }

    // GET: /Products/Edit/5
    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<IActionResult> Edit(
        int id,
        CancellationToken token)
    {
        try
        {
            var product = await _productApiService.GetByIdAsync(id, token);

            if (product == null)
                return NotFound();

            var details = await _productDetailsApiService.GetByIdAsync(
                id, token);

            var model = new ProductEditPageModel
            {
                Product = new ProductUpdateModel
                {
                    Name = product.Name,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                },

                HasDetails = details != null,

                Details = details == null
                    ? new ProductDetailsUpdateModel()
                    : new ProductDetailsUpdateModel
                    {
                        Description = details.Description,
                        Manufacturer = details.Manufacturer,
                        WarrantyMonths = details.WarrantyMonths
                    }
            };

            // Prepare manufacturer Select.
            if (details != null)
            {
                var manufacturers = new[]
                {
                    "Apple",
                    "Samsung",
                    "Dell",
                    "HP",
                    "Lenovo",
                    "Logitech"
                };

                if (manufacturers.Contains(details.Manufacturer))
                {
                    model.ManufacturerOption = details.Manufacturer;
                }
                else
                {
                    model.ManufacturerOption = "Other";
                    model.CustomManufacturer = details.Manufacturer;
                }
            }

            ViewBag.ProductId = id;

            return View(model);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Unable to load product {ProductId} for editing.",
                id);

            TempData["Error"] = "Unable to load product for editing.";

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: /Products/Edit/5
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        ProductEditPageModel model,
        CancellationToken token)
    {
        ViewBag.ProductId = id;

        // Manufacturer is selected through ManufacturerOption
        // or entered manually through CustomManufacturer.
        ModelState.Remove("Details.Manufacturer");

        string? manufacturer = null;

        if (model.HasDetails)
        {
            manufacturer = model.ManufacturerOption switch
            {
                "Other" => model.CustomManufacturer?.Trim(),

                "Apple" or "Samsung" or "Dell" or "HP" or
                "Lenovo" or "Logitech" => model.ManufacturerOption,

                _ => null
            };

            if (string.IsNullOrWhiteSpace(manufacturer))
            {
                ModelState.AddModelError(
                    "ManufacturerOption",
                    "Please select or enter a manufacturer.");
            }
            else
            {
                // Map selected value to ProductDetails.
                model.Details.Manufacturer = manufacturer;
            }
        }
        else
        {
            // ProductDetails are disabled.
            ModelState.Remove("Details.Manufacturer");
            ModelState.Remove("Details.Description");
            ModelState.Remove("Details.WarrantyMonths");
        }

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            // 1. Update Product.
            using var productResponse =
                await _productApiService.UpdateAsync(
                    id,
                    model.Product,
                    token);

            if (!productResponse.IsSuccessStatusCode)
            {
                var error =
                    await productResponse.Content.ReadAsStringAsync(token);

                ModelState.AddModelError(
                    string.Empty,
                    string.IsNullOrWhiteSpace(error)
                        ? $"Product update failed: {(int)productResponse.StatusCode}."
                        : error);

                return View(model);
            }

            // 2. Check existing ProductDetails.
            var existingDetails =
                await _productDetailsApiService.GetByIdAsync(id, token);

            if (!model.HasDetails)
            {
                // Delete ProductDetails when checkbox is disabled.
                if (existingDetails != null)
                {
                    using var deleteResponse =
                        await _productDetailsApiService.DeleteAsync(
                            id,
                            token);

                    if (!deleteResponse.IsSuccessStatusCode &&
                        deleteResponse.StatusCode != HttpStatusCode.NotFound)
                    {
                        TempData["Error"] =
                            "Product was updated, but additional details could not be deleted.";

                        return RedirectToAction(nameof(Edit), new { id });
                    }
                }
            }
            else if (existingDetails == null)
            {
                // 3. Create ProductDetails if they don't exist.
                var createModel = new ProductDetailsCreateModel
                {
                    ProductId = id,
                    Description = model.Details.Description,
                    Manufacturer = model.Details.Manufacturer,
                    WarrantyMonths = model.Details.WarrantyMonths
                };

                using var detailsResponse =
                    await _productDetailsApiService.CreateAsync(
                        createModel,
                        token);

                if (!detailsResponse.IsSuccessStatusCode)
                {
                    var error =
                        await detailsResponse.Content.ReadAsStringAsync(token);

                    TempData["Error"] =
                        $"Product updated, but additional details could not be created. {error}";

                    return RedirectToAction(nameof(Edit), new { id });
                }
            }
            else
            {
                // 4. Update existing ProductDetails.
                using var detailsResponse =
                    await _productDetailsApiService.UpdateAsync(
                        id,
                        model.Details,
                        token);

                if (!detailsResponse.IsSuccessStatusCode)
                {
                    var error =
                        await detailsResponse.Content.ReadAsStringAsync(token);

                    TempData["Error"] =
                        $"Product updated, but additional details could not be updated. {error}";

                    return RedirectToAction(nameof(Edit), new { id });
                }
            }

            TempData["Success"] = "Product updated successfully.";

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Unable to update product {ProductId}.",
                id);

            ModelState.AddModelError(
                string.Empty,
                "Unable to connect to the API.");

            return View(model);
        }
    }

    // POST: /Products/Delete/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken token)
    {
        try
        {
            using var response = await _productApiService.DeleteAsync(
                id, token);

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
                var error =
                    await response.Content.ReadAsStringAsync(token);

                TempData["Error"] = string.IsNullOrWhiteSpace(error)
                    ? $"Unable to delete product. API returned {(int)response.StatusCode}."
                    : error;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Unable to delete product {ProductId}.",
                id);

            TempData["Error"] = "Unable to connect to the API.";
        }

        return RedirectToAction(nameof(Index));
    }
}
