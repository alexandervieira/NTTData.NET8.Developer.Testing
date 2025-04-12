using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, 
                              IStockService stockService, IMapper mapper)
        {
            _productRepository = productRepository;
            _stockService = stockService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponse>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductResponse>>(await _productRepository.GetAll());
        }

        public async Task<IEnumerable<ProductResponse>> GetByCategory(int code)
        {
            return _mapper.Map<IEnumerable<ProductResponse>>(await _productRepository.GetByCategory(code));
        }

        public async Task<ProductResponse> GetById(Guid id)
        {
            return _mapper.Map<ProductResponse>(await _productRepository.GetById(id));
        }       

        public async Task<IEnumerable<CategoryResponse>> GetCategories()
        {
            return _mapper.Map<IEnumerable<CategoryResponse>>(await _productRepository.GetCategories());
        }

        public Task<bool> AddProduct(ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            _productRepository.AddProduct(product);
            return _productRepository.UnitOfWork.CommitAsync();
        }

        public Task<bool> UpdateProduct(ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            _productRepository.UpdateProduct(product);
            return _productRepository.UnitOfWork.CommitAsync();
        }

        public async Task<ProductResponse> DebitStock(Guid id, int quantity)
        {
            if (!await _stockService.DebitStock(id, quantity))
            {
                throw new DomainException("Failed to debit stock.");
            }
            return _mapper.Map<ProductResponse>(await _productRepository.GetById(id));
        }             

        public async Task<ProductResponse> ReplenishStock(Guid id, int quantity)
        {
            if (!await _stockService.ReplenishStock(id, quantity))
            {
                throw new DomainException("Failed to replenish stock.");
            }
            return _mapper.Map<ProductResponse>(await _productRepository.GetById(id));
        }        

        public void Dispose()
        {
            _productRepository.Dispose();
            _stockService.Dispose();
        }
    }
}
