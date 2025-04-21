using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Domain.Events.Catalog;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Services
{
    public class ProductService : IProductService
    {        
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public ProductService(IProductRepository productRepository, IStockService stockService, 
                              IMapper mapper, IMediatorHandler mediatorHandler)
        {
            _productRepository = productRepository;
            _stockService = stockService;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<PaginatedList<ProductResponse>> GetAll(int pageNumber, int pageSize, string? query)
        {
            return _mapper.Map<PaginatedList<ProductResponse>>(await _productRepository.GetAll(pageNumber, pageSize, query));
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

        public async Task<ProductResponse> AddProduct(CreateProductRequest request)
        {
            var product = _mapper.Map<Product>(request);            
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            // Iniciar transação no PostgreSQL
            using var transaction = await _productRepository.BeginTransactionAsync();

            try
            {

                var model = await _productRepository.AddProduct(product);
                if (model == null)
                {
                    throw new DomainException("Failed to add product.");
                }

                var result = await _productRepository.UnitOfWork.CommitAsync();
                if (!result)
                {
                    throw new DomainException("Failed to commit transaction.");
                }

                var productTomongo = await _productRepository.GetById(model.Id);
                // Publicar evento de domínio
                await _mediatorHandler.PublishDomainEvent(new ProductCreatedEvent(productTomongo));               

                // Confirmar transação no PostgreSQL
                await transaction.CommitAsync();

                var response = _mapper.Map<ProductResponse>(model);
                return response;

            }
            catch (Exception)
            {
                // Reverter transação no PostgreSQL em caso de erro
                await transaction.RollbackAsync();
                throw;
            }                    
                        
        }

        public async Task<ProductResponse> UpdateProduct(UpdateProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            var model = await _productRepository.UpdateProduct(product);

            if (model == null || !await _productRepository.UnitOfWork.CommitAsync())
            {
                throw new DomainException("Failed to update product.");
            }

            var response = _mapper.Map<ProductResponse>(model);
            return response;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            _productRepository.DeleteProduct(id);
            return await _productRepository.UnitOfWork.CommitAsync();
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

        public async Task<IEnumerable<ProductResponse>> GetByCategoryName(string categoryName)
        {
            if(string.IsNullOrEmpty(categoryName))
                throw new ArgumentNullException(nameof(categoryName));
            
            var model = await _productRepository.GetByCategoryName(categoryName);
            if (model == null) return null;                
            var response = _mapper.Map<IEnumerable<ProductResponse>>(model);
            return response;
        }

        public async Task<CategoryResponse> AddCategory(CategoryRequest request)
        {
            var category = _mapper.Map<Category>(request);
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var model = await _productRepository.AddCategory(category);
            if (model == null || !await _productRepository.UnitOfWork.CommitAsync())
            {
                throw new DomainException("Failed to add category.");
            }
            var response = _mapper.Map<CategoryResponse>(model);
            return response;
        }

        public async Task<CategoryResponse> UpdateCategory(CategoryRequest request)
        {
            var category = _mapper.Map<Category>(request);
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var model = await _productRepository.UpdateCategory(category);
            if (model == null || !await _productRepository.UnitOfWork.CommitAsync())
            {
                throw new DomainException("Failed to update category.");
            }
            var response = _mapper.Map<CategoryResponse>(model);
            return response;
        }
    }
    
}
