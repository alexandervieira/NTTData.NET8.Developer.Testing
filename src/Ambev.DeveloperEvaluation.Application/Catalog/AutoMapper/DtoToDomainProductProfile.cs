using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.AutoMapper
{
    public class DtoToDomainProductProfile : Profile
    {
        public DtoToDomainProductProfile()
        {
            CreateMap<CreateProductRequest, Product>()
                .ForPath(dest => dest.Rating.Rate, opt => opt.MapFrom(src => src.Rating.Rate))
                .ForPath(dest => dest.Rating.Count, opt => opt.MapFrom(src => src.Rating.Count))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                .ForPath(dest => dest.Dimensions.Height, opt => opt.MapFrom(src => src.Dimensions.Height))
                .ForPath(dest => dest.Dimensions.Width, opt => opt.MapFrom(src => src.Dimensions.Width))
                .ForPath(dest => dest.Dimensions.Depth, opt => opt.MapFrom(src => src.Dimensions.Depth));

            CreateMap<UpdateProductRequest, Product>()
                .ForPath(dest => dest.Rating.Rate, opt => opt.MapFrom(src => src.Rating.Rate))
                .ForPath(dest => dest.Rating.Count, opt => opt.MapFrom(src => src.Rating.Count))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                .ForPath(dest => dest.Dimensions.Height, opt => opt.MapFrom(src => src.Dimensions.Height))
                .ForPath(dest => dest.Dimensions.Width, opt => opt.MapFrom(src => src.Dimensions.Width))
                .ForPath(dest => dest.Dimensions.Depth, opt => opt.MapFrom(src => src.Dimensions.Depth));              

            CreateMap<CategoryRequest, Category>()
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}

