using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Catalog.Services
{
    public interface IProductServiceMongo
    {
        Task<PaginatedList<ProductResponse>> GetAllAsync(int pageNumber, int pageSize, string? query, string order);
    }
}
