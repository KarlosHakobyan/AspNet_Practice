using AspNet.BLL.Common.Errors;
using AspNet.BLL.Common.Results;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductModels;

namespace AspNet.BLL.Services.Products;

public class ProductService(
    IProductRepository repository,
    ICategoryRepository categoryRepository,
    IUnitOfWork uow) : IProductService
{
    private readonly IProductRepository _repository = repository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<Result<IEnumerable<Product>>> GetAllAsync(CancellationToken token = default)
    {
        var products = await _repository.GetAllAsync(token);

        return Result<IEnumerable<Product>>.Success(products);
    }

    public async Task<Result<Product>> GetByIdAsync(int id,CancellationToken token = default)
    {
        var product = await _repository.GetByIdAsync(id, token);

        if (product is null)
            return Result<Product>.Failure(ProductErrors.NotFound);

        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> GetWithCategoryAsync(int id,CancellationToken token = default)
    {
        var product = await _repository.GetWithCategoryAsync(id, token);

        if (product is null)
            return Result<Product>.Failure(ProductErrors.NotFound);

        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> GetFullAsync(int id,CancellationToken token = default)
    {
        var product = await _repository.GetFullAsync(id, token);

        if (product is null)
            return Result<Product>.Failure(ProductErrors.NotFound);

        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> GetWithDetailsAsync(int id,CancellationToken token = default)
    {
        var product = await _repository.GetWithDetailsAsync(id, token);

        if (product is null)
            return Result<Product>.Failure(ProductErrors.NotFound);

        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> CreateAsync(ProductAddModel model,CancellationToken token = default)
    {
        var category = await _categoryRepository.GetByIdAsync(model.CategoryId,token);

        if (category is null)
            return Result<Product>.Failure(ProductErrors.CategoryNotFound);

        var product = new Product
        {
            Name = model.Name,
            Price = model.Price,
            CategoryId = model.CategoryId
        };

        await _repository.AddAsync(product, token);

        await _uow.SaveChangesAsync(token);

        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> UpdateAsync(int id,ProductUpdateModel model,CancellationToken token = default)
    {
        var product = await _repository.GetByIdAsync(id, token);

        if (product is null)
            return Result<Product>.Failure(ProductErrors.NotFound);

        var category = await _categoryRepository.GetByIdAsync(model.CategoryId,token);

        if (category is null)
            return Result<Product>.Failure(ProductErrors.CategoryNotFound);

        product.Name = model.Name;
        product.Price = model.Price;
        product.CategoryId = model.CategoryId;

        await _repository.UpdateAsync(product, token);

        await _uow.SaveChangesAsync(token);

        return Result<Product>.Success(product);
    }

    public async Task<Result> DeleteAsync(int id,CancellationToken token = default)
    {
        var product = await _repository.GetByIdAsync(id, token);

        if (product is null)
            return Result.Failure(ProductErrors.NotFound);

        await _repository.DeleteAsync(id, token);

        await _uow.SaveChangesAsync(token);

        return Result.Success();
    }
}