using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Application.Catalog.Validations;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Catalog;

/// <summary>
/// Controller for managing catalog operations
/// </summary>
[ApiController]
[Route("api/catalog")]
public class CatalogsController : BaseController
{
    //private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Initializes a new instance of catalogsController
    /// </summary>
    /// <param name="mediatorHandler">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CatalogsController(IMapper mapper, IProductService productService, IMediatorHandler mediatorHandler, IDistributedCache cache)
    {
        _mapper = mapper;
        _productService = productService;
        _mediatorHandler = mediatorHandler;
        _cache = cache;
    }

    [HttpGet("getproducts")]
    [ProducesResponseType(typeof(PaginatedResponse<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? query = null, [FromQuery] string order = "title asc")
    {
        var response = await _productService.GetAllAsync(pageNumber, pageSize, query, order);

        if (response == null || !response.Any())
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Products not found."
            });

        return Ok(new PaginatedResponse<ProductResponse>
        {
            Success = true,
            Message = "Products retrieved successfully",
            Data = response,
            CurrentPage = pageNumber,
            TotalPages = response.TotalPages,
            TotalCount = response.TotalCount
        });
    }

    /// <summary>
    /// Retrieves products
    /// </summary>    
    /// <returns>The products if found</returns>
    [HttpGet("products")]
    [ProducesResponseType(typeof(PaginatedResponse<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8, [FromQuery] string? query = null)
    {
        var cacheKey = $"products_{pageNumber}_{pageSize}_{query}";
        var cachedProducts = await _cache.GetStringAsync(cacheKey);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        if (!string.IsNullOrEmpty(cachedProducts))
        {
            var products = JsonSerializer.Deserialize<PaginatedResponse<ProductResponse>>(cachedProducts, options);

            return Ok(new ApiResponseWithData<PaginatedResponse<ProductResponse>>
            {
                Success = true,
                Message = "Products retrieved successfully (from cache)",
                Data = products
            });
        }

        var response = await _productService.GetAll(pageNumber, pageSize, query);

        if (response == null || !response.Any())
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Products not found."
            });
        }

        var paginatedResponse = new Common.PaginatedResponse<ProductResponse>
        {
            Success = true,
            Message = "Products retrieved successfully",
            Data = response,
            CurrentPage = response.CurrentPage,
            TotalPages = response.TotalPages,
            TotalCount = response.TotalCount
        };

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(paginatedResponse, options), cacheOptions);

        return Ok(paginatedResponse);
    }


    /// <summary>
    /// Retrieves a product by ID
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    [HttpGet("product-detail/{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetProductRequest { Id = id };
        var validator = new GetProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await _productService.GetById(request.Id);

        return Ok(new ApiResponseWithData<ProductResponse>
        {
            Success = true,
            Message = "Product retrieved successfully",
            Data = response
        });
    }

}
