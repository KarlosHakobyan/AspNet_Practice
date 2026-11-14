using AspNet.BLL.Common.Errors;
using AspNet.BLL.Common.Results;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductDetailsModels;

namespace AspNet.BLL.Services.Products;

public class ProductDetailsService(
    IProductDetailsRepository repository,
    IProductRepository productRepository,
    IUnitOfWork uow) : IProductDetailsService
{
    private readonly IProductDetailsRepository _repository = repository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<Result<ProductDetails>> GetByIdAsync(int productId,CancellationToken token = default)
    {
        var details = await _repository.GetByIdAsync(productId, token);

        if (details is null)
            return Result<ProductDetails>.Failure(ProductDetailsErrors.NotFound);

        return Result<ProductDetails>.Success(details);
    }

    public async Task<Result<ProductDetails>> CreateAsync(ProductDetailsAddModel model,CancellationToken token = default)
    {
        var product = await _productRepository.GetByIdAsync(model.ProductId,token);

        if (product is null)
            return Result<ProductDetails>.Failure(ProductDetailsErrors.ProductNotFound);

        var existingDetails = await _repository.GetByIdAsync(model.ProductId,token);

        if (existingDetails is not null)
            return Result<ProductDetails>.Failure(ProductDetailsErrors.AlreadyExists);

        var details = new ProductDetails
        {
            ProductId = model.ProductId,
            Description = model.Description,
            Manufacturer = model.Manufacturer,
            WarrantyMonths = model.WarrantyMonths
        };

        await _repository.AddAsync(details, token);
        await _uow.SaveChangesAsync(token);

        return Result<ProductDetails>.Success(details);
    }

    public async Task<Result<ProductDetails>> UpdateAsync(int productId,ProductDetailsUpdateModel model,CancellationToken token = default)
    {
        var details = await _repository.GetByIdAsync(productId, token);

        if (details is null)
            return Result<ProductDetails>.Failure(ProductDetailsErrors.NotFound);

        details.Description = model.Description;
        details.Manufacturer = model.Manufacturer;
        details.WarrantyMonths = model.WarrantyMonths;

        await _repository.UpdateAsync(details, token);
        await _uow.SaveChangesAsync(token);

        return Result<ProductDetails>.Success(details);
    }

    public async Task<Result> DeleteAsync(int productId,CancellationToken token = default)
    {
        var details = await _repository.GetByIdAsync(productId, token);

        if (details is null)
            return Result.Failure(ProductDetailsErrors.NotFound);

        await _repository.DeleteAsync(productId, token);
        await _uow.SaveChangesAsync(token);

        return Result.Success();
    }
}