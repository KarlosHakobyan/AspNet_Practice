using AspNet.Data.Repositories.Interfaces;
using AspNet.Data.Repositories.UnitOfWork;
using AspNet.Domain.Entities;
using AspNet.Domain.Models.ProductDetailsModels;

namespace AspNet.Api.Services.Products
{
    public class ProductDetailsService(IProductDetailsRepository repository, IUnitOfWork uow) : IProductDetailsService
    {
        private readonly IProductDetailsRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;

        public async Task<ProductDetails?> GetByIdAsync(int productId, CancellationToken token = default)
        {
            return await _repository.GetByIdAsync(productId, token);
        }

        public async Task<ProductDetails?> CreateAsync(ProductDetailsAddModel model, CancellationToken token = default)
        {
            var details = new ProductDetails
            {
                ProductId = model.ProductId,
                Description = model.Description,
                Manufacturer = model.Manufacturer,
                WarrantyMonths = model.WarrantyMonths
            };

            await _repository.AddAsync(details, token);
            await _uow.SaveChangesAsync(token);

            return details;
        }

        public async Task<ProductDetails?> UpdateAsync(int productId, ProductDetailsUpdateModel model, CancellationToken token = default)
        {
            var details = await _repository.GetByIdAsync(productId, token);

            if (details == null)
                return null;

            details.Description = model.Description;
            details.Manufacturer = model.Manufacturer;
            details.WarrantyMonths = model.WarrantyMonths;

            await _repository.UpdateAsync(details, token);
            await _uow.SaveChangesAsync(token);

            return details;
        }

        public async Task<bool> DeleteAsync(int productId, CancellationToken token = default)
        {
            var details = await _repository.GetByIdAsync(productId, token);

            if (details == null)
                return false;

            await _repository.DeleteAsync(productId, token);
            await _uow.SaveChangesAsync(token);

            return true;
        }
    }
}