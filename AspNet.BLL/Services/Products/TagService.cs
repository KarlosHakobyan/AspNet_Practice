using AspNet.BLL.Common.Errors;
using AspNet.BLL.Common.Results;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.TagModels;

namespace AspNet.BLL.Services.Products;

public class TagService(
    ITagRepository repository,
    IUnitOfWork uow) : ITagService
{
    private readonly ITagRepository _repository = repository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<Result<IEnumerable<Tag>>> GetAllAsync(CancellationToken token = default)
    {
        var tags = await _repository.GetAllAsync(token);

        return Result<IEnumerable<Tag>>.Success(tags);
    }

    public async Task<Result<Tag>> GetByIdAsync(int id,CancellationToken token = default)
    {
        var tag = await _repository.GetByIdAsync(id, token);

        if (tag is null)
            return Result<Tag>.Failure(TagErrors.NotFound);

        return Result<Tag>.Success(tag);
    }

    public async Task<Result<Tag>> CreateAsync(TagAddModel model,CancellationToken token = default)
    {
        var tag = new Tag
        {
            Name = model.Name
        };

        await _repository.AddAsync(tag, token);
        await _uow.SaveChangesAsync(token);

        return Result<Tag>.Success(tag);
    }

    public async Task<Result<Tag>> UpdateAsync(int id,TagUpdateModel model,CancellationToken token = default)
    {
        var tag = await _repository.GetByIdAsync(id, token);

        if (tag is null)
            return Result<Tag>.Failure(TagErrors.NotFound);

        tag.Name = model.Name;

        await _repository.UpdateAsync(tag, token);
        await _uow.SaveChangesAsync(token);

        return Result<Tag>.Success(tag);
    }

    public async Task<Result> DeleteAsync(int id,CancellationToken token = default)
    {
        var tag = await _repository.GetByIdAsync(id, token);

        if (tag is null)
            return Result.Failure(TagErrors.NotFound);

        var isUsed = await _repository.IsUsedAsync(id, token);

        if (isUsed)
            return Result.Failure(TagErrors.IsInUse);

        await _repository.DeleteAsync(id, token);
        await _uow.SaveChangesAsync(token);

        return Result.Success();
    }
}