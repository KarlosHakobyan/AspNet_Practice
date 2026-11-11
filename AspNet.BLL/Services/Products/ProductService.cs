using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductModels;


namespace AspNet.BLL.Services.Products
{
    public class ProductService(IProductRepository repository, ICategoryRepository categoryRepository, IUnitOfWork uow) : IProductService
    {
        private readonly IProductRepository _repository = repository;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _uow = uow;

        public async Task<IEnumerable<Product>> GetAllAsync(
            CancellationToken token = default)
        {
            return await _repository.GetAllAsync(token);
        }

        public async Task<Product?> GetByIdAsync(
            int id,
            CancellationToken token = default)
        {
            return await _repository.GetByIdAsync(id, token);
        }

        public async Task<Product?> GetWithCategoryAsync(
            int id,
            CancellationToken token = default)
        {
            return await _repository.GetWithCategoryAsync(id, token);
        }

        public async Task<Product?> GetFullAsync(
            int id,
            CancellationToken token = default)
        {
            return await _repository.GetFullAsync(id, token);
        }

        public async Task<Product?> GetWithDetailsAsync(
            int id,
            CancellationToken token = default)
        {
            return await _repository.GetWithDetailsAsync(id, token);
        }

        public async Task<bool> CreateAsync(
            ProductAddModel model,
            CancellationToken token = default)
        {
            var category = await _categoryRepository.GetByIdAsync(
                model.CategoryId,
                token);

            if (category == null)
                return false;

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId
            };

            await _repository.AddAsync(product, token);
            await _uow.SaveChangesAsync(token);

            return true;
        }

        public async Task<bool> UpdateAsync(
            int id,
            ProductUpdateModel model,
            CancellationToken token = default)
        {
            var product = await _repository.GetByIdAsync(id, token);

            if (product == null)
                return false;

            var category = await _categoryRepository.GetByIdAsync(
                model.CategoryId,
                token);

            if (category == null)
                return false;

            product.Name = model.Name;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;

            await _repository.UpdateAsync(product, token);
            await _uow.SaveChangesAsync(token);

            return true;
        }

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken token = default)
        {
            var product = await _repository.GetByIdAsync(id, token);

            if (product == null)
                return false;

            await _repository.DeleteAsync(id, token);
            await _uow.SaveChangesAsync(token);

            return true;
        }
    }
}
