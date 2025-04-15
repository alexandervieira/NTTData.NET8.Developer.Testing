using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.ORM.Context;
using Ambev.DeveloperEvoluation.Core.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Catalog
{
    public class ProductRepository : IProductRepository
    {
        private readonly DefaultContext _context;
        private readonly IProductContext _mongoContext;
        
        public ProductRepository(DefaultContext context, IProductContext ctx)
        {
            _context = context;
            _mongoContext = ctx;
        }

        public IUnitOfWork UnitOfWork => _context;       

        public async Task<PaginatedList<Product>> GetAllAsync(int pageNumber, int pageSize, string query, string order)
        {
            var filter = Builders<Product>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(query))
            {
                // Filtro com regex (LIKE)
                filter = Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Regex(p => p.Title, new BsonRegularExpression(query, "i")),
                    Builders<Product>.Filter.Regex(p => p.Description, new BsonRegularExpression(query, "i"))
                );
            }

            // Ordenação
            var sort = Builders<Product>.Sort.Ascending(order);
            if (order.Contains("desc", StringComparison.OrdinalIgnoreCase))
            {
                sort = Builders<Product>.Sort.Descending(order.Replace(" desc", "", StringComparison.OrdinalIgnoreCase));
            }

            var products = await _mongoContext.Products.Find(filter)
                                        .Sort(sort)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync();

            var count = await _mongoContext.Products.CountDocumentsAsync(filter);

            return new PaginatedList<Product>(products, (int)count, pageNumber, pageSize);
        }


        public async Task<PaginatedList<Product>> GetAll(int pageNumber, int pageSize, string query)
        {
            string newquery = query != null ? query.ToLower() : string.Empty;
            var catalogQuery = _context.Products.AsQueryable();

            var source = catalogQuery.AsNoTrackingWithIdentityResolution()
                                            .Where(x => EF.Functions.Like(x.Title.ToLower(), $"%{newquery}%"))
                                            .OrderBy(x => x.Title).AsQueryable();

            return await PaginatedList<Product>.CreateAsync(source, pageNumber, pageSize);
            
        }

        public async Task<Product?> GetById(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);           

            return product;
        }

        public async Task<IEnumerable<Product>> GetByCategory(int code)
        {
            return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.Category.Code == code)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> AddProduct(Product product)
        {
            var result = await _context.Products.AddAsync(product);           
            await _context.SaveChangesAsync();            
            return result.Entity;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = _context.Products.Update(product);
            await _context.SaveChangesAsync();            
            return result.Entity;
        }

        public void DeleteProduct(Guid id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) throw new ArgumentNullException(nameof(product));
            _context.Products.Remove(product);            
            
        }

        public async Task<Category> AddCategory(Category category)
        {
            var result = _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            var result = _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<Product>> GetByCategoryName(string categoryName)
        {           
            string query = categoryName != null ? categoryName.ToLower() : string.Empty;
            var catalogQuery = _context.Products.AsQueryable();
            var source = catalogQuery.AsNoTrackingWithIdentityResolution()
                                     .Include(p => p.Category)
                                     .Where(x => EF.Functions.Like(x.Category.Name.ToLower(), $"%{query}%"))
                                     .OrderBy(x => x.Title).AsQueryable();
            return await source.ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        
    }
    
}
