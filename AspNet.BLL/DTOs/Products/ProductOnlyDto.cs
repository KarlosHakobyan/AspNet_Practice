using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.BLL.DTOs.Products
{
    public class ProductOnlyDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public decimal Price { get; set; }
    }
}
