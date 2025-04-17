using Ambev.DeveloperEvaluation.Unit.Fixture;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    [Collection(nameof(ProductCollection))]
    public class ProductTests
    {
        private readonly ProductFixtureTests _productFixture;

        public ProductTests(ProductFixtureTests productFixture)
        {
            _productFixture = productFixture;
        }

        [Fact(DisplayName = "Canceled Product")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_Cancel_MustCancelProduct()
        {
            //Arrange
            var product = _productFixture.CreateValidProduct();

            //Act
            product?.Cancel();

            //Assert            
            Assert.False(product?.Active);
        }

        [Fact(DisplayName = "Activated Product")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_Active_MustActivateProduct()
        {
            //Arrange
            var product = _productFixture.CreateProducts(1, true).First();

            //Act
            product?.Activate();

            //Assert            
            Assert.True(product?.Active);
        }

        [Fact(DisplayName = "Create Product")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_Create_ProductMustBeValid()
        {
            //Arrange
            var product = _productFixture.CreateValidProduct();

            //Act
            var result = product?.Validate();

            //Assert            
            result?.IsValid.Should().BeTrue();
        }

        [Fact(DisplayName = "Invalid Product")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_Create_ProductInValidMustBeFalse()
        {
            //Arrange
            var product = _productFixture.CreateInvalidProduct();

            //Act
            var result = product?.Validate();

            //Assert            
            result?.IsValid.Should().BeFalse();
        }

        [Fact(DisplayName = "DebitStock Product")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_DebitStock_MustSubtractQuantityOfProducts()
        {
            //Arrange
            var product = _productFixture.CreateValidProduct();            ;
            var qualityStockRecived = product.QuantityStock;

            //Act
            product.DebitStock(1);

            //Assert            
            Assert.True(product.QuantityStock < qualityStockRecived);
        }

        [Fact(DisplayName = "ReplenishStock Product")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_ReplenishStock_MustToAddQuantityOfProducts()
        {
            //Arrange
            var product = _productFixture.CreateValidProduct();
            var qualityStockRecived = product.QuantityStock;

            //Act
            product.ReplenishStock(15);

            //Assert            
            Assert.True(product.QuantityStock > qualityStockRecived);
        }

        [Fact(DisplayName = "HasStock Product")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_HasStock_ShouldReturnTrue()
        {
            //Arrange
            var product = _productFixture.CreateValidProduct();
            product.ReplenishStock(10);

            //Act
            var result = product.HasStock(2);

            //Assert            
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Not HasStock Product")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_NotHasStock_ShouldThrowException()
        {
            //Arrange
            var product = _productFixture.CreateValidProduct();            

            //Act
            var ex = Assert.ThrowsAny<DomainException>(() => product.DebitStock(51));
            
            //Assert            
            Assert.Equal("Estoque insuficiente", ex.Message);
        }

        [Fact(DisplayName = "Update Product Category")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_Update_ProductCategoryWithSuccess()
        {
            //Arrange
            var product = _productFixture.CreateValidProduct();
            var category = _productFixture.CreateCategory();

            //Act
            product.UpdateCategory(category);

            //Assert            
            product.Category.Should().BeEquivalentTo(category);
        }

        [Fact(DisplayName = "Update Product Description")]
        [Trait("Category", "Product Bogus Tests")]
        public void Product_Update_ProductDescriptionWithSuccess()
        {
            //Arrange
            var product = _productFixture.CreateValidProduct();
            var description = _productFixture.GenerateProductDescription();

            //Act
            product.UpdateDescription(description);

            //Assert            
            product.Description.Should().BeEquivalentTo(description);
        }

    }

}
