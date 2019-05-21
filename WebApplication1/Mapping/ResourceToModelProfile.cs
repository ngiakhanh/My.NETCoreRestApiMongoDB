using AutoMapper;
using WebApplication1.Domain.Models;
using WebApplication1.Resources;

namespace WebApplication1.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveCategoryResource, Category>();
            CreateMap<SaveProductResource, Product>();
        }
    }
}