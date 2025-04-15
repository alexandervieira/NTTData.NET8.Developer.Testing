using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Context
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> ProductCollection)
        {
            bool exist = ProductCollection.Find(p => true).Any();
            if (!exist)
            {
                ProductCollection.InsertMany(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>() 
            {
                new Product("Refrigerante", 5.0m, false),
                new Product("Cerveja", 10.0m, true),
                new Product("Suco", 3.0m, false),
                new Product("Água", 1.0m, false),
                new Product("Energético", 7.0m, true),
                new Product("Chá", 2.0m, false),
                new Product("Isotônico", 4.0m, false),

            };
        }
    }
}
