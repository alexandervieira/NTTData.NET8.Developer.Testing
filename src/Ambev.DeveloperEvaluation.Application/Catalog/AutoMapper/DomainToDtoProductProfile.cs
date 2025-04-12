using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using AutoMapper;
using System.Globalization;

namespace Ambev.DeveloperEvaluation.Application.Catalog.AutoMapper
{
    public class DomainToDtoProductProfile : Profile
    {
        public DomainToDtoProductProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.ToString("C", CultureInfo.CurrentCulture)))
                .ForMember(dest => dest.QuantityStock, opt => opt.MapFrom(src => src.QuantityStock))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rating.Rate))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Rating.Count))
                .ForMember(dest => dest.Canceled, opt => opt.MapFrom(src => src.Canceled))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Dimensions.Height.ToString("N2")))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Dimensions.Width.ToString("N2")))
                .ForMember(dest => dest.Depth, opt => opt.MapFrom(src => src.Dimensions.Depth.ToString("N2")));             

            CreateMap<Category, CategoryResponse>();
            
        }
    }
    
}
