using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using Ambev.DeveloperEvoluation.Core.Data;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Catalog
{
    public class ProductServiceTests
    {
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IStockService _stockService = Substitute.For<IStockService>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepository.UnitOfWork.Returns(_unitOfWork);
            _productService = new ProductService(_productRepository, _stockService, _mapper);
        }

        [Fact(DisplayName = "Must return all products successfully")]
        public async Task ProductService_GetAll_MustReturnAllProductsSuccessfully()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("Cerveja", 10.0m, false)
            };
            var productsResponse = new List<ProductResponse>
            {
                new ProductResponse { Title = "Cerveja", Price = 10.0m }
            };

            _productRepository.GetAll().Returns(Task.FromResult<IEnumerable<Product>>(products));
            _mapper.Map<IEnumerable<ProductResponse>>(products).Returns(productsResponse);

            // Act
            var resultado = await _productService.GetAll();

            // Assert
            resultado.Should().BeEquivalentTo(productsResponse);
            await _productRepository.Received(1).GetAll();
            _mapper.Received(1).Map<IEnumerable<ProductResponse>>(products);
        }

        [Fact(DisplayName = "Must return products by category")]
        public async Task ProductService_GetByCategory_MustReturnProductsByCategory()
        {
            // Arrange
            int codeCategory = 1;
            var products = new List<Product>
            {
                new Product("Refrigerante", 5.0m, false)
            };
            var productsResponse = new List<ProductResponse>
            {
                new ProductResponse { Title = "Refrigerante", Price = 5.0m }
            };

            _productRepository.GetByCategory(codeCategory).Returns(Task.FromResult<IEnumerable<Product>>(products));
            _mapper.Map<IEnumerable<ProductResponse>>(products).Returns(productsResponse);

            // Act
            var resultado = await _productService.GetByCategory(codeCategory);

            // Assert
            resultado.Should().BeEquivalentTo(productsResponse);
            await _productRepository.Received(1).GetByCategory(codeCategory);
            _mapper.Received(1).Map<IEnumerable<ProductResponse>>(products);
        }

        [Fact(DisplayName = "Must return product by ID")]
        public async Task ProductService_GetById_MustReturnProductByID()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new Product("Água", 2.0m, false);
            var productResponse = new ProductResponse { Title = "Água", Price = 2.0m };

            _productRepository.GetById(id).Returns(Task.FromResult(product));
            _mapper.Map<ProductResponse>(product).Returns(productResponse);

            // Act
            var resultado = await _productService.GetById(id);

            // Assert
            resultado.Should().BeEquivalentTo(productResponse);
            await _productRepository.Received(1).GetById(id);
            _mapper.Received(1).Map<ProductResponse>(product);
        }

        [Fact(DisplayName = "Should retrieve all categories successfully")]
        public async Task GetCategories_ShouldRetrieveAllCategories()
        {
            // Arrange
            var categories = new List<Category> { new Category("Beverages", 1) };
            var categoryResponses = new List<CategoryResponse> { new CategoryResponse { Name = "Beverages", Code = 1 } };

            _productRepository.GetCategories().Returns(Task.FromResult<IEnumerable<Category>>(categories));
            _mapper.Map<IEnumerable<CategoryResponse>>(categories).Returns(categoryResponses);

            // Act
            var result = await _productService.GetCategories();

            // Assert
            result.Should().BeEquivalentTo(categoryResponses);
            await _productRepository.Received(1).GetCategories();
            _mapper.Received(1).Map<IEnumerable<CategoryResponse>>(categories);
        }

        [Fact(DisplayName = "Should add a product successfully")]
        public async Task AddProduct_ShouldAddProductSuccessfully()
        {
            // Arrange
            var request = new ProductRequest { Title = "Beer", Price = 10.0m };
            var product = new Product("Beer", 10.0m, false);

            _mapper.Map<Product>(request).Returns(product);
            _unitOfWork.CommitAsync().Returns(Task.FromResult(true));

            // Act
            var result = await _productService.AddProduct(request);

            // Assert
            result.Should().BeTrue();
            _productRepository.Received(1).AddProduct(product);
            await _unitOfWork.Received(1).CommitAsync();
        }

        [Fact(DisplayName = "Should update a product successfully")]
        public async Task UpdateProduct_ShouldUpdateProductSuccessfully()
        {
            // Arrange
            var request = new ProductRequest { Title = "Soda", Price = 5.0m };
            var product = new Product("Soda", 5.0m, false);

            _mapper.Map<Product>(request).Returns(product);
            _unitOfWork.CommitAsync().Returns(Task.FromResult(true));

            // Act
            var result = await _productService.UpdateProduct(request);

            // Assert
            result.Should().BeTrue();
            _productRepository.Received(1).UpdateProduct(product);
            await _unitOfWork.Received(1).CommitAsync();
        }

        [Fact(DisplayName = "Should debit stock successfully")]
        public async Task DebitStock_ShouldDebitStockSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var quantity = 5;
            var product = new Product("Water", 2.0m, false);
            var productResponse = new ProductResponse { Title = "Water", Price = 2.0m };

            _stockService.DebitStock(id, quantity).Returns(Task.FromResult(true));
            _productRepository.GetById(id).Returns(Task.FromResult(product));
            _mapper.Map<ProductResponse>(product).Returns(productResponse);

            // Act
            var result = await _productService.DebitStock(id, quantity);

            // Assert
            result.Should().BeEquivalentTo(productResponse);
            await _stockService.Received(1).DebitStock(id, quantity);
            await _productRepository.Received(1).GetById(id);
            _mapper.Received(1).Map<ProductResponse>(product);
        }

        [Fact(DisplayName = "Should throw exception when failing to debit stock")]
        public async Task DebitStock_ShouldThrowExceptionWhenFailingToDebitStock()
        {
            // Arrange
            var id = Guid.NewGuid();
            var quantity = 5;

            _stockService.DebitStock(id, quantity).Returns(Task.FromResult(false));

            // Act
            Func<Task> act = async () => await _productService.DebitStock(id, quantity);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Failed to debit stock.");
            await _stockService.Received(1).DebitStock(id, quantity);
        }

        [Fact(DisplayName = "Should replenish stock successfully")]
        public async Task ReplenishStock_ShouldReplenishStockSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var quantity = 10;
            var product = new Product("Juice", 3.0m, false);
            var productResponse = new ProductResponse { Title = "Juice", Price = 3.0m };

            _stockService.ReplenishStock(id, quantity).Returns(Task.FromResult(true));
            _productRepository.GetById(id).Returns(Task.FromResult(product));
            _mapper.Map<ProductResponse>(product).Returns(productResponse);

            // Act
            var result = await _productService.ReplenishStock(id, quantity);

            // Assert
            result.Should().BeEquivalentTo(productResponse);
            await _stockService.Received(1).ReplenishStock(id, quantity);
            await _productRepository.Received(1).GetById(id);
            _mapper.Received(1).Map<ProductResponse>(product);
        }

        [Fact(DisplayName = "Should throw exception when failing to replenish stock")]
        public async Task ReplenishStock_ShouldThrowExceptionWhenFailingToReplenishStock()
        {
            // Arrange
            var id = Guid.NewGuid();
            var quantity = 10;

            _stockService.ReplenishStock(id, quantity).Returns(Task.FromResult(false));

            // Act
            Func<Task> act = async () => await _productService.ReplenishStock(id, quantity);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Failed to replenish stock.");
            await _stockService.Received(1).ReplenishStock(id, quantity);
        }
    }
}
