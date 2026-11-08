using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductModels;

namespace AspNet.Api.Services.Products
{
    public class ProductService(IProductRepository repository, IUnitOfWork uow) : IProductService
    {
        public readonly IProductRepository _repository = repository;
        public readonly IUnitOfWork _uow = uow;

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
        {
            return await _repository.GetAllAsync(token);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _repository.GetByIdAsync(id, token);
        }

        public async Task<Product?> GetWithCategoryAsync(int id, CancellationToken token = default)
        {
            return await _repository.GetWithCategoryAsync(id, token);

        }

        public async Task<Product?> GetFullAsync(int id, CancellationToken token = default)
        {
            return await _repository.GetFullAsync(id, token);
        }

        public Task<Product?> GetWithDetailsAsync(int id, CancellationToken token = default)
        {
            return _repository.GetWithDetailsAsync(id, token);
        }



        public async Task CreateAsync(ProductAddModel model, CancellationToken token = default)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId
            };
            await _repository.AddAsync(product, token);
            await _uow.SaveChangesAsync(token);

        }

        public async Task<Product?> UpdateAsync(int id, ProductUpdateModel model, CancellationToken token = default)
        {
            var product = await _repository.GetByIdAsync(id, token);

            if (product == null)
                return null;

            product.Name = model.Name;
            product.Price = model.Price;

            await _repository.UpdateAsync(product, token);
            await _uow.SaveChangesAsync(token);

            return product;
        }



        public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
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

