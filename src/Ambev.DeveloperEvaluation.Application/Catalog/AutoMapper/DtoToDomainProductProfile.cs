using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.AutoMapper
{
    internal class DtoToDomainProductProfile : Profile
    {
        public DtoToDomainProductProfile()
        {
            CreateMap<ProductRequest, Product>();
            
            CreateMap<CategoryRequest, Category>()
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
    
}
