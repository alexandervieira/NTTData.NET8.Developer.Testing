using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Services
{
    public class ProductServiceMongo : IProductServiceMongo
    {
        private readonly IProductRepositoryMongo _productRepository;
        private readonly IMapper _mapper;

        public ProductServiceMongo(IProductRepositoryMongo productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProductResponse>> GetAllAsync(int pageNumber, int pageSize, string? query, string order)
        {
            return _mapper.Map<PaginatedList<ProductResponse>>(await _productRepository.GetAllAsync(pageNumber, pageSize, query, order));
        }
    }
}
