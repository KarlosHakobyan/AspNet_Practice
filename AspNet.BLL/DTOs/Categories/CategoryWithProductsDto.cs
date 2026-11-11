using AspNet.BLL.DTOs.Products;
using AspNet.Domain.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.BLL.DTOs.Categories
{
    public class CategoryWithProductsDto
    {
            public int Id { get; set; }

            public string Name { get; set; } = default!;

            public List<ProductOnlyDto> Products { get; set; }

     }


}
