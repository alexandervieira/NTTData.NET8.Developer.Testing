using Ambev.DeveloperEvaluation.Core.DomainObjects.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.ORM.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Catalog
{
    public class ProductRepositoryMongo : IProductRepositoryMongo
    {
        private readonly IProductContext _mongoContext;

        public ProductRepositoryMongo(IProductContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<PaginatedList<MongoProduct>> GetAllAsync(int pageNumber, int pageSize, string? query, string order)
        {
            var filter = Builders<MongoProduct>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(query))
            {
                // Filtro com regex (LIKE)
                filter = Builders<MongoProduct>.Filter.Or(
                    Builders<MongoProduct>.Filter.Regex(p => p.Title, new BsonRegularExpression(query, "i")),
                    Builders<MongoProduct>.Filter.Regex(p => p.Description, new BsonRegularExpression(query, "i"))
                );
            }

            // Ordenação
            var sort = Builders<MongoProduct>.Sort.Ascending(order);
            if (order.Contains("desc", StringComparison.OrdinalIgnoreCase))
            {
                sort = Builders<MongoProduct>.Sort.Descending(order.Replace(" desc", "", StringComparison.OrdinalIgnoreCase));
            }

            var products = await _mongoContext.Products.Find(filter)
                                        .Sort(sort)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync();

            var count = await _mongoContext.Products.CountDocumentsAsync(filter);

            return new PaginatedList<MongoProduct>(products, (int)count, pageNumber, pageSize);
        }

        public async Task AddProductToMongo(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

           var mongoProduct = new MongoProduct(                
                product.CategoryId,
                product.Title,                
                product.Price,
                product.Active,
                product.Description,
                product.Image,
                product.QuantityStock,
                new Rating(product.Rating?.Rate ?? 0,
                product.Rating?.Count ?? 0),               
                new Dimensions(product.Dimensions?.Width ?? 0,
                product.Dimensions?.Height ?? 0,
                product.Dimensions?.Depth ?? 0)                
           );

            mongoProduct.Id = product.Id;
            mongoProduct.CreatedAt = product.CreatedAt;
            mongoProduct.UpdatedAt = product.UpdatedAt;

            // Adiciona o produto ao MongoDB
            await _mongoContext.Products.InsertOneAsync(mongoProduct);
            
        }

        public async Task AddCategoryToMongo(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var mongoCategory = new MongoCategory(                
                category.Name,
                category.Code                
            );
            
            mongoCategory.Id = category.Id;
            mongoCategory.CreatedAt = category.CreatedAt;
            mongoCategory.UpdatedAt = category.UpdatedAt;
            
            var result = _mongoContext.Categories                 
                                      .FindSync(c => c.Id == mongoCategory.Id)
                                      .FirstOrDefaultAsync();            

            if (result.Result == null) {
                await _mongoContext.Categories.InsertOneAsync(mongoCategory);
            }
          
        }

    }
}
