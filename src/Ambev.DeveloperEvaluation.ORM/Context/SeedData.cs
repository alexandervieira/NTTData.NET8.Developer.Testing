using Ambev.DeveloperEvaluation.Core.DomainObjects.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Context
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<MongoProduct> ProductCollection)
        {
            // Criação das categorias
            var shirtCategory = new MongoCategory("Camisas", 100);
            var mugCategory = new MongoCategory("Canecas", 101);
            var smartphoneCategory = new MongoCategory("Smartphone", 102);
            var iphoneCategory = new MongoCategory("Iphone", 103);

            var categories = new Dictionary<Guid, MongoCategory>
            {
                { shirtCategory.Id, shirtCategory },
                { mugCategory.Id, mugCategory },
                { smartphoneCategory.Id, smartphoneCategory },
                { iphoneCategory.Id, iphoneCategory }
            };

            categories.Values.ToList().ForEach(p => p.CreatedAt = DateTime.UtcNow);

            // Persiste categorias (se ainda não existem)
            var categoryCollection = ProductCollection.Database.GetCollection<MongoCategory>("Categories");
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

        private static IEnumerable<MongoProduct> GetPreconfiguredProducts(Dictionary<Guid, MongoCategory> categories)
        {
            var iphoneId = categories.First(c => c.Value.Name == "Iphone").Key;
            var smartphoneId = categories.First(c => c.Value.Name == "Smartphone").Key;
            var shirtId = categories.First(c => c.Value.Name == "Camisas").Key;
            var mugId = categories.First(c => c.Value.Name == "Canecas").Key;

            var products = new List<MongoProduct>
            {
                new MongoProduct(iphoneId,     "IPhone",               2998.00M, true, "Aliquam erat volutpat", "iphone.png",      50, new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new MongoProduct(smartphoneId, "Samsung Galaxy S4",    989.00M, true,  "Aliquam erat volutpat", "galaxy-s4.jpg",   50, new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new MongoProduct(smartphoneId, "Samsung Galaxy Note",  1179.00M, true, "Aliquam erat volutpat", "galaxy-note.jpg", 50, new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new MongoProduct(smartphoneId, "Z1",                   1089.00M, true, "Aliquam erat volutpat", "Z1.png",          50, new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new MongoProduct(shirtId,      "Camiseta Developer",   99.00M, true,   "Camiseta 100% algodão", "Camiseta1.jpg",   50, new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new MongoProduct(shirtId,      "Camiseta Code",        89.00M, true,   "Camiseta 100% algodão", "camiseta2.jpg",   50, new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new MongoProduct(mugId,        "Caneca StarBugs",      49.00M, true,   "Aliquam erat volutpat", "caneca1.jpg",     50, new Rating(2.9, 10), new Dimensions(5, 5, 5)),
                new MongoProduct(mugId,        "Caneca Code",          45.00M, true,   "Aliquam erat volutpat", "caneca2.jpg",     50, new Rating(2.9, 10), new Dimensions(5, 5, 5))
            };
                        
            products.ForEach(p => p.CreatedAt = DateTime.UtcNow);
            products.ForEach(p => p.UpdatedAt = null);

            return products;
        }
    }
}
