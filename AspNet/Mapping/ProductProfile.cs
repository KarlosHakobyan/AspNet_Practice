using AspNet.Entities;
using AspNet.Models.DTOs.Categories;
using AspNet.Models.DTOs.ProductDetails;
using AspNet.Models.DTOs.Products;
using AspNet.Models.DTOs.Tags;
using AutoMapper;

namespace AspNet.Mapping
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
