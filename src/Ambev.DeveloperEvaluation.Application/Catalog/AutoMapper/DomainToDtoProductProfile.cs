using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.AutoMapper
{
    public class DomainToDtoProductProfile : Profile
    {
        public DomainToDtoProductProfile()
        {
            CreateMap<Product, ProductResponse>()               
                .ForMember(dest => dest.Rating.Rate, opt => opt.MapFrom(src => src.Rating.Rate))
                .ForMember(dest => dest.Rating.Count, opt => opt.MapFrom(src => src.Rating.Count))
                .ForMember(dest => dest.Canceled, opt => opt.MapFrom(src => src.Canceled))
                .ForMember(dest => dest.Dimensions.Height, opt => opt.MapFrom(src => src.Dimensions.Height))
                .ForMember(dest => dest.Dimensions.Width, opt => opt.MapFrom(src => src.Dimensions.Width))
                .ForMember(dest => dest.Dimensions.Depth, opt => opt.MapFrom(src => src.Dimensions.Depth));             

            CreateMap<Category, CategoryResponse>();
            
        }
    }
    
}
