using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.AutoMapper
{
    public class DomainToDtoProductProfile : Profile
    {
        public DomainToDtoProductProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForPath(dest => dest.CreatedAtFormatted,
               opt => opt.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy")))
                  .ForPath(dest => dest.UpdatedAtFormatted,
               opt => opt.MapFrom(src => src.UpdatedAt.Value.ToString("dd/MM/yyyy")));
            
            CreateMap<Category, CategoryResponse>();
            
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
               .ConvertUsing(typeof(PaginatedListConverter<,>));
        }
    }

    public class PaginatedListConverter<TSource, TDestination> : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        public PaginatedList<TDestination> Convert(PaginatedList<TSource> source, PaginatedList<TDestination> destination, ResolutionContext context)
        {
            var mappedItems = context.Mapper.Map<List<TDestination>>(source.ToList());
            return new PaginatedList<TDestination>(mappedItems, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }

}
