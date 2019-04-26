using AutoMapper;
using WebApplication1.Domain.Models;
using WebApplication1.Extensions;
using WebApplication1.Resources;

namespace WebApplication1.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Category, CategoryResource>();
            CreateMap<Product, ProductResource>()
                .ForMember(dest => dest.UnitOfMeasurement,
                           opt => opt.MapFrom(src => src.UnitOfMeasurement.ToDescriptionString()));
        }
    }
}
