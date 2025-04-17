
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities;
using Bogus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Fixture
{
    [CollectionDefinition(nameof(ProductCollection))]
    public class ProductCollection : ICollectionFixture<ProductFixtureTests> { }
    public class ProductFixtureTests : IDisposable
    {
        private bool _disposedValue = false;

        public IEnumerable<Product> CreateProducts(int quantity, bool active)
        {
            var bogus = new Faker<Product>("pt_BR")
            .CustomInstantiator(f => new Product(
                f.Random.Guid(), // CategoryId
                f.Commerce.ProductName(), // Title
                f.Random.Decimal(49M, 1000M), // Price               
                active,
                f.Commerce.ProductDescription(), // Description
                f.Image.PicsumUrl(), // Image
                new Rating(
                    f.Random.Double(1, 5), // Rate
                    f.Random.Int(0, 5000)  // Count
                ),
                new Dimensions(
                    f.Random.Double(1, 100), // Width
                    f.Random.Double(1, 100), // Height
                    f.Random.Double(1, 100)  // Depth
                )                
            ));

            return bogus.Generate(quantity);
        }

        public Product CreateInvalidProduct()
        {
            var bogus = new Faker<Product>("pt_BR");
            bogus.CustomInstantiator(f => new Product(
               f.Random.Guid(), // CategoryId
               string.Empty,
               f.Random.Decimal(49M, 1000M),               
               f.Commerce.ProductDescription() // Description
            ));
            return bogus.Generate();
        }

        public Product CreateValidProduct()
        {
            return CreateProducts(1, false).First();
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            var products = new List<Product>();
            products.Add(CreateValidProduct());
            products.Add(CreateProducts(1, true).First());
            return Task.FromResult(products.AsEnumerable());
        }

        public string GenerateProductDescription()
        {
            var faker = new Faker("pt_BR");
            return faker.Commerce.ProductDescription();
        }

        public Category CreateCategory()
        {
            var bogus = new Faker<Category>("pt_BR");
            bogus.CustomInstantiator(f => new Category(
               f.Commerce.Department(),
               f.Random.Int(1, 100)
            ));
            return bogus.Generate();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                _disposedValue = true;
            }
        }

        ~ProductFixtureTests()
        {
            Dispose(false);
        }
    }
}
