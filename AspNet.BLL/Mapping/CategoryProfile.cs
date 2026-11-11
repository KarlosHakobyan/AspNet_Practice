using AspNet.BLL.DTOs.Categories;
using AspNet.Domain.Entities;
using AutoMapper;

namespace AspNet.BLL.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryWithProductsDto>();
               
        }
    }
}
