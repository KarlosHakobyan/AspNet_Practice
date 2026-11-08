using AspNet.Api.Models.DTOs.Categories;
using AspNet.Api.Models.DTOs.ProductDetails;
using AspNet.Api.Models.DTOs.Products;
using AspNet.Api.Models.DTOs.Tags;
using AspNet.Domain.Entities;
using AutoMapper;

namespace AspNet.Api.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {

            CreateMap<Product, ProductDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<ProductDetails, ProductDetailsDto>();
            CreateMap<Tag, TagDto>();

            CreateMap<Product, ProductFullDto>()
             .ForMember(
                dest => dest.Tags,
                opt => opt.MapFrom(src =>
                src.ProductTags.Where(pt => pt.Tag != null)
                .Select(pt => pt.Tag!)));

        }
    }
}
