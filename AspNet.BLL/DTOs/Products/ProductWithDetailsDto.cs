using AspNet.BLL.DTOs.ProductDetails;
using AspNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.BLL.DTOs.Products
{
    public class ProductWithDetailsDto : ProductDto
    {
        public ProductDetailsDto? Details { get; set; }
    }
}
