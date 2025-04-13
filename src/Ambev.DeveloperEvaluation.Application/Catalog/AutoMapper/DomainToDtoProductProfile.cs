using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.AutoMapper
{
    public class DomainToDtoProductProfile : Profile
    {
        public DomainToDtoProductProfile()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<Category, CategoryResponse>();
        }
    }
    
}
