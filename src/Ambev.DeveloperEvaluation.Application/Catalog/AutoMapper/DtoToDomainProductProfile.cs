using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.AutoMapper
{
    public class DtoToDomainProductProfile : Profile
    {
        public DtoToDomainProductProfile()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
            CreateMap<CategoryRequest, Category>()
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}

