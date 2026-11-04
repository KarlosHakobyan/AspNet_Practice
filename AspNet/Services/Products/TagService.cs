using AspNet.Entities;
using AspNet.Models.TagModels;
using AspNet.Repositories.Interfaces;
using AspNet.Repositories.UnitOfWork;

namespace AspNet.Services.Products
{
    public class TagService(ITagRepository repository, IUnitOfWork uow) : ITagService
    {
        public readonly ITagRepository _repository = repository;
        public readonly IUnitOfWork _uow = uow;

        public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken token = default)
        {
            return await _repository.GetAllAsync(token);
        }
    
        public async Task<Tag?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _repository.GetByIdAsync(id, token);
        }

        public async Task<Tag> CreateAsync(TagAddModel model, CancellationToken token = default)
        {
            var tag = new Tag
            {
                Name = model.Name
            };
            await _repository.AddAsync(tag, token);
            await _uow.SaveChangesAsync(token);
            return tag;
        }

        public async Task<Tag?> UpdateAsync(int id, TagUpdateModel model, CancellationToken token = default)
        {
            var tag = await _repository.GetByIdAsync(id, token);
            if (tag == null)
                return null;

            tag.Name = model.Name;
            await _repository.UpdateAsync(tag, token);
            await _uow.SaveChangesAsync(token);

            return tag;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
        {
            var tag = await _repository.GetByIdAsync(id, token);

            if (tag == null)
                return false;

            var isUsed = await _repository.IsUsedAsync(id, token);

            if (isUsed)
                return false;

            await _repository.DeleteAsync(id, token);
            await _uow.SaveChangesAsync(token);

            return true;
        }

    }

}
