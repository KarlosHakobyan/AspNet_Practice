using AspNet.Data;
using AspNet.Data.Repositories.Generic;
using AspNet.Data.Repositories.Interfaces;
using AspNet.Domain.Entities;

namespace AspNet.Data.Repositories
{
    public class ProductDetailsRepository(ApplicationDbContext context) : GenericRepository<ProductDetails>(context), IProductDetailsRepository
    {

    }
}
