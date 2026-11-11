using AspNet.BLL.DTOs.Categories;
using AspNet.BLL.DTOs.ProductDetails;
using AspNet.BLL.DTOs.Products;
using AspNet.BLL.DTOs.Tags;
using AspNet.Domain.Entities;
using AutoMapper;

namespace AspNet.BLL.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductOnlyDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<ProductDetails, ProductDetailsDto>();
            CreateMap<Tag, TagDto>();

            CreateMap<Category, CategoryWithProductsDto>();
            CreateMap<Product, ProductWithDetailsDto>();


            CreateMap<Product, ProductFullDto>()
                .ForMember(
                   dest => dest.Tags,
                  opt => opt.MapFrom(src =>
                  src.ProductTags.Where(pt => pt.Tag != null)
                 .Select(pt => pt.Tag!)));
        }
    }
}
