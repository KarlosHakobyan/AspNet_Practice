using AspNet.BLL.Common.Results;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.TagModels;

namespace AspNet.BLL.Services.Products;

public interface ITagService
{
    Task<Result<IEnumerable<Tag>>> GetAllAsync(CancellationToken token = default);

    Task<Result<Tag>> GetByIdAsync(int id,CancellationToken token = default);

    Task<Result<Tag>> CreateAsync(TagAddModel model,CancellationToken token = default);

    Task<Result<Tag>> UpdateAsync(int id,TagUpdateModel model,CancellationToken token = default);

    Task<Result> DeleteAsync(int id,CancellationToken token = default);
}