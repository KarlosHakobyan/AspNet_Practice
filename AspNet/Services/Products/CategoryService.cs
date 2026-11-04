using AspNet.Entities;
using AspNet.Models.CategoryModels;
using AspNet.Repositories.Interfaces;
using AspNet.Repositories.UnitOfWork;

namespace AspNet.Services.Products
{
    public class CategoryService (ICategoryRepository repository , IUnitOfWork uow) : ICategoryService
    {
        public readonly ICategoryRepository _repository = repository;
        public readonly IUnitOfWork _uow = uow;

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken token = default)
        {
            return await _repository.GetAllAsync(token);
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _repository.GetByIdAsync(id, token);
        }

        public async Task CreateAsync(CategoryAddModel model, CancellationToken token = default)
        {
            var category = new Category
            {
                Name = model.Name
            };
            await _repository.AddAsync(category, token);
            await _uow.SaveChangesAsync(token);
        }

        public async Task<Category?> GetWithProductsAsync(int categoryId, CancellationToken token = default)
        {
            return await _repository.GetWithProductsAsync(categoryId, token);
        }

        public async Task<Category?> UpdateAsync(int id, CategoryUpdateModel model, CancellationToken token = default)
        {
            var category = await _repository.GetByIdAsync(id, token);

            if (category == null)
                return null;

            category.Name = model.Name;
            await _repository.UpdateAsync(category, token);
            await _uow.SaveChangesAsync(token);

            return category;
        }

        public async Task<bool> DeleteAsync(int id , CancellationToken token)
        {
            var category = await _repository.GetByIdAsync(id, token);

            if (category == null)
                return false;

            await _repository.DeleteAsync(id, token);
            await _uow.SaveChangesAsync(token);

            return true;
        }
    }
}
