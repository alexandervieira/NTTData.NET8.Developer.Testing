using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvoluation.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Catalog
{
    public class ProductRepository : IProductRepository
    {
        private readonly DefaultContext _context;        
        public ProductRepository(DefaultContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
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

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<Product>> GetByCategoryName(string categoryName)
        {
            var result = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.Category.Name.ToUpper().Equals(categoryName.ToUpper()))
                .ToListAsync();
            return result;
        }
    }
    
}
