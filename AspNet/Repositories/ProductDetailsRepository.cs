
using AspNet.Data;
using AspNet.Entities;
using AspNet.Repositories.Generic;
using AspNet.Repositories.Interfaces;

namespace AspNet.Repositories
{
    public class ProductDetailsRepository(ApplicationDbContext context) : GenericRepository<ProductDetails>(context) , IProductDetailsRepository
    {
                
    }
}
