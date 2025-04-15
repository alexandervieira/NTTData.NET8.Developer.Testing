using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
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
            _mapper = Substitute.For<IMapper>();
            _productRepository = Substitute.For<IProductRepository>();
            _productRepository.UnitOfWork.Returns(_unitOfWork);            
            _stockService = Substitute.For<IStockService>();            
            _productService = new ProductService(_productRepository, _stockService, _mapper);
        }

        [Fact(DisplayName = "Must return all products")]
        public async Task ProductService_GetAll_MustReturnAllProducts()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var query = "test";
            var products = new List<Product>
            {
                new Product("Product 1", 10.0m, true),
                new Product("Product 2", 20.0m, true)
            };

            var paginatedProducts = new PaginatedList<Product>(products, products.Count, pageNumber, pageSize);

            _productRepository.GetAll(pageNumber, pageSize, query).Returns(Task.FromResult(paginatedProducts));
            _mapper.Map<PaginatedList<ProductResponse>>(paginatedProducts).Returns(new PaginatedList<ProductResponse>(
                new List<ProductResponse>
                {
                    new ProductResponse { Title = "Product 1", Description = "Description 1", Price = 10.0m },
                    new ProductResponse { Title = "Product 2", Description = "Description 2", Price = 20.0m }
                }, products.Count, pageNumber, pageSize));

            // Act
            var result = await _productService.GetAll(pageNumber, pageSize, query);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("Product 1");
            result[1].Title.Should().Be("Product 2");
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
        public async Task ProductService_GetCategories_ShouldRetrieveAllCategories()
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
        public async Task ProductService_AddProduct_ShouldAddProductSuccessfully()
        {
            // Arrange
            var category = new Category("Test Category", 1);
            var request = new CreateProductRequest
            {
                CategoryId = Guid.NewGuid(),
                Title = "Test Product",
                Description = "Test Description"
            };
            var product = new Product(request.Title, request.Description, false, 100, request.CategoryId, 
                                      "http://imagem.jpg", new Rating(2.9,10), new Dimensions(10,10,10));
            var addedProduct = product;
            var AddedCategory = category;
            _mapper.Map<Product>(request).Returns(product);            
            _productRepository.AddCategory(category).Returns(Task.FromResult(AddedCategory));
            _productRepository.AddProduct(product).Returns(Task.FromResult(addedProduct));
            _mapper.Map<ProductResponse>(addedProduct).Returns(new ProductResponse { Id = addedProduct.Id, Title = addedProduct.Title });

            // Act
            var result = await _productService.AddProduct(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(addedProduct.Id, result.Id);
            Assert.Equal(addedProduct.Title, result.Title);
            await _productRepository.Received(1).AddProduct(product);
        }       

        [Fact(DisplayName = "Should update a product successfully")]
        public async Task ProductService_UpdateProduct_ShouldUpdateProductSuccessfully()
        {
            // Arrange
            var request = new UpdateProductRequest
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                Title = "Updated Product",
                Description = "Updated Description",
                Price = 200,
                Active = false,
                Image = "updated_image.png",
                QuantityStock = 50,
                Rating = new Rating(4.5, 10),
                Dimensions = new Dimensions(10, 20, 30)
            };

            var product = new Product(request.Title, request.Description, request.Active, request.Price, request.CategoryId,
                                      request.Image, request.Rating, request.Dimensions);

            var updatedProduct = product;
            var image = updatedProduct.Image != null ? updatedProduct.Image : string.Empty;
            var rate = updatedProduct.Rating != null ? updatedProduct.Rating.Rate : 0;
            var ratingCount = updatedProduct.Rating != null ? updatedProduct.Rating.Count : 0;
            var height = updatedProduct.Dimensions != null ? updatedProduct.Dimensions.Height : 0;
            var width = updatedProduct.Dimensions != null ? updatedProduct.Dimensions.Width : 0;
            var depth = updatedProduct.Dimensions != null ? updatedProduct.Dimensions.Depth : 0;

            _mapper.Map<Product>(request).Returns(product);
            _productRepository.UpdateProduct(product).Returns(Task.FromResult(updatedProduct));
            _mapper.Map<ProductResponse>(updatedProduct).Returns(new ProductResponse
            {
                Id = updatedProduct.Id,
                Title = updatedProduct.Title,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                Image = image,
                QuantityStock = updatedProduct.QuantityStock,
                Rating = new Rating(rate, ratingCount),
                Dimensions = new Dimensions(height, width, depth),
                Active = updatedProduct.Active
            });

            // Act
            var result = await _productService.UpdateProduct(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedProduct.Id, result.Id);
            Assert.Equal(updatedProduct.Title, result.Title);
            Assert.Equal(updatedProduct.Description, result.Description);
            Assert.Equal(updatedProduct.Price, result.Price);
            Assert.Equal(updatedProduct.Image, result.Image);
            Assert.Equal(updatedProduct.QuantityStock, result.QuantityStock);
            Assert.Equal(rate, result.Rating.Rate);
            Assert.Equal(ratingCount, result.Rating.Count);
            Assert.Equal(height, result.Dimensions.Height);
            Assert.Equal(width, result.Dimensions.Width);
            Assert.Equal(depth, result.Dimensions.Depth);
            Assert.Equal(updatedProduct.Active, result.Active);
            await _productRepository.Received(1).UpdateProduct(product);
        }

        [Fact(DisplayName = "Should debit stock successfully")]
        public async Task ProductService_DebitStock_ShouldDebitStockSuccessfully()
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
        public async Task ProductService_DebitStock_ShouldThrowExceptionWhenFailingToDebitStock()
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
        public async Task ProductService_ReplenishStock_ShouldReplenishStockSuccessfully()
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
        public async Task ProductService_ReplenishStock_ShouldThrowExceptionWhenFailingToReplenishStock()
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
