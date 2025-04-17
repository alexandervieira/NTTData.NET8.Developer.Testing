using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Context
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> ProductCollection)
        {
            // Criação das categorias
            var shirtCategory = new Category("Camisas", 100);
            var mugCategory = new Category("Canecas", 101);
            var smartphoneCategory = new Category("Smartphone", 102);
            var iphoneCategory = new Category("Iphone", 103);

            var categories = new Dictionary<Guid, Category>
            {
                { shirtCategory.Id, shirtCategory },
                { mugCategory.Id, mugCategory },
                { smartphoneCategory.Id, smartphoneCategory },
                { iphoneCategory.Id, iphoneCategory }
            };

            categories.Values.ToList().ForEach(p => p.CreatedAt = DateTime.UtcNow);

            // Persiste categorias (se ainda não existem)
            var categoryCollection = ProductCollection.Database.GetCollection<Category>("Categories");
            bool categoriesExist = categoryCollection.Find(c => true).Any();
            if (!categoriesExist)
            {
                categoryCollection.InsertMany(categories.Values);
            }

            // Persiste produtos (se ainda não existem)
            bool exist = ProductCollection.Find(p => true).Any();
            if (!exist)
            {
                var products = GetPreconfiguredProducts(categories);
                ProductCollection.InsertMany(products);
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts(Dictionary<Guid, Category> categories)
        {
            var iphoneId = categories.First(c => c.Value.Name == "Iphone").Key;
            var smartphoneId = categories.First(c => c.Value.Name == "Smartphone").Key;
            var shirtId = categories.First(c => c.Value.Name == "Camisas").Key;
            var mugId = categories.First(c => c.Value.Name == "Canecas").Key;

            var products = new List<Product>
            {
                new Product(iphoneId, "Aliquam erat volutpat", 2998.00M, true, "IPhone", "iphone.png", new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new Product(smartphoneId, "Aliquam erat volutpat", 989.00M, true, "Samsung Galaxy S4", "galaxy-s4.jpg", new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new Product(smartphoneId, "Aliquam erat volutpat", 1179.00M, true, "Samsung Galaxy Note", "galaxy-note.jpg", new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new Product(smartphoneId, "Aliquam erat volutpat", 1089.00M, true, "Z1", "Z1.png", new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new Product(shirtId, "Camiseta 100% algodão", 99.00M, true, "Camiseta Developer", "Camiseta1.jpg", new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new Product(shirtId, "Camiseta 100% algodão", 89.00M, true, "Camiseta Code", "camiseta2.jpg", new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new Product(mugId, "Aliquam erat volutpat", 49.00M, true, "Caneca StarBugs", "caneca1.jpg", new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new Product(mugId, "Aliquam erat volutpat", 45.00M, true, "Caneca Code", "caneca2.jpg", new Rating(2.9, 10), new Dimensions(5, 5, 5))
            };

            products.ForEach(p => p.CreatedAt = DateTime.UtcNow);

            return products;
        }

    }
}
