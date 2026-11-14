using AspNet.BLL.Common.Errors;
using AspNet.BLL.Common.Results;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.CategoryModels;

namespace AspNet.BLL.Services.Products;

public class CategoryService(
    ICategoryRepository repository,
    IUnitOfWork uow) : ICategoryService
{
    private readonly ICategoryRepository _repository = repository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<Result<IEnumerable<Category>>> GetAllAsync(CancellationToken token = default)
    {
        var categories = await _repository.GetAllAsync(token);

        return Result<IEnumerable<Category>>.Success(categories);
    }

    public async Task<Result<Category>> GetByIdAsync(int id,CancellationToken token = default)
    {
        var category = await _repository.GetByIdAsync(id, token);

        if (category is null)
            return Result<Category>.Failure(CategoryErrors.NotFound);

        return Result<Category>.Success(category);
    }

    public async Task<Result<Category>> GetWithProductsAsync(int categoryId,CancellationToken token = default)
    {
        var category = await _repository.GetWithProductsAsync(categoryId,token);

        if (category is null)
            return Result<Category>.Failure(CategoryErrors.NotFound);

        return Result<Category>.Success(category);
    }

    public async Task<Result<Category>> CreateAsync(CategoryAddModel model,CancellationToken token = default)
    {
        var category = new Category
        {
            Name = model.Name
        };

        await _repository.AddAsync(category, token);
        await _uow.SaveChangesAsync(token);

        return Result<Category>.Success(category);
    }

    public async Task<Result<Category>> UpdateAsync(int id,CategoryUpdateModel model,CancellationToken token = default)
    {
        var category = await _repository.GetByIdAsync(id, token);

        if (category is null)
            return Result<Category>.Failure(CategoryErrors.NotFound);

        category.Name = model.Name;

        await _repository.UpdateAsync(category, token);
        await _uow.SaveChangesAsync(token);

        return Result<Category>.Success(category);
    }

    public async Task<Result> DeleteAsync(int id,CancellationToken token = default)
    {
        var category = await _repository.GetByIdAsync(id, token);

        if (category is null)
            return Result.Failure(CategoryErrors.NotFound);

        await _repository.DeleteAsync(id, token);
        await _uow.SaveChangesAsync(token);

        return Result.Success();
    }
}