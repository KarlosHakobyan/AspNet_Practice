using AspNet.BLL.DTOs.ProductDetails;
using AspNet.Domain.Entities;
using AutoMapper;

namespace AspNet.BLL.Mapping
{
    internal class ProductDetailsProfile : Profile
    {
        public ProductDetailsProfile()
        {
            CreateMap<ProductDetails, ProductDetailsDto>();
        }
    }
}
