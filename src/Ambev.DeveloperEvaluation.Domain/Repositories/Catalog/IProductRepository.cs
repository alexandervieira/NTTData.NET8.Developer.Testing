﻿using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvoluation.Core.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Catalog
{
    public interface IProductRepository : IRepository<Product>
    {            
        Task<PaginatedList<Product>> GetAll(int pageNumber, int pageSize, string? query);
        Task<Product?> GetById(Guid id);
        Task<IEnumerable<Product>> GetByCategory(int code);
        Task<IEnumerable<Product>> GetByCategoryName(string categoryName);
        Task<IEnumerable<Category>> GetCategories();

        Product UpdateDetach(Product product);
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        void DeleteProduct(Guid id);

        Task<Category> AddCategory(Category category);
        Task<Category> UpdateCategory(Category category);

        Task<IDbContextTransaction> BeginTransactionAsync();        

    }
}
