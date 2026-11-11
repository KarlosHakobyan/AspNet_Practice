using AspNet.BLL.DTOs.Tag;
using AspNet.Domain.Entities;
using AutoMapper;

namespace AspNet.BLL.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>();


        }
    }
}
