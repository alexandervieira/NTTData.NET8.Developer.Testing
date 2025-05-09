﻿using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvoluation.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Catalog
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {        
                
        public ProductRepository(DefaultContext context) : base(context) { }

        public IUnitOfWork UnitOfWork => (DefaultContext)_context;       

        public async Task<PaginatedList<Product>> GetAll(int pageNumber, int pageSize, string? query)
        {
            string newquery = query != null ? query.ToLower() : string.Empty;
            var catalogQuery = _dbSet.AsQueryable();

            var source = catalogQuery.AsNoTrackingWithIdentityResolution()
                                            .Where(x => EF.Functions.Like(x.Title.ToLower(), $"%{newquery}%"))
                                            .OrderBy(x => x.Title).AsQueryable();

            return await PaginatedList<Product>.CreateAsync(source, pageNumber, pageSize);
            
        }

        public async Task<Product?> GetById(Guid id)
        {
            var product = await _dbSet
                .Include(p => p.Category)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(p => p.Id == id);           

            return product;
        }

        public async Task<IEnumerable<Product>> GetByCategory(int code)
        {
            return await _dbSet
                .Include(p => p.Category)
                .AsNoTrackingWithIdentityResolution()
                .Where(p => p.Category.Code == code)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories =  await _context.Set<Category>()
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();
            return categories;
        }

        public async Task<Product> AddProduct(Product product)
        {
            var result = await _dbSet.AddAsync(product);
            return result.Entity;
        }

        public Task<Product> UpdateProduct(Product product)
        {
            var result = _dbSet.Update(product);
            return Task.FromResult(product);
        }

        public Product UpdateDetach(Product product)
        {
            return Update(product);            
        }              

        public void DeleteProduct(Guid id)
        {
            var product = _dbSet.FirstOrDefault(p => p.Id == id);
            if (product == null) throw new ArgumentNullException(nameof(product));
            _dbSet.Remove(product);            
            
        }

        public async Task<Category> AddCategory(Category category)
        {
            var result = await _context.Set<Category>().AddAsync(category);            
            return result.Entity;
        }

        public Task<Category> UpdateCategory(Category category)
        {
            var result = _context.Set<Category>().Update(category);            
            return Task.FromResult(result.Entity);
        }

        public async Task<IEnumerable<Product>> GetByCategoryName(string categoryName)
        {           
            string query = categoryName != null ? categoryName.ToLower() : string.Empty;
            var catalogQuery = _dbSet.AsQueryable();
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

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        
    }
    
}
