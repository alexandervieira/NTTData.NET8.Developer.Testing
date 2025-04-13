using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Application.Catalog.Validations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Catalog;

/// <summary>
/// Controller for managing catalog operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CatalogsController : BaseController
{
    //private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private readonly IMediatorHandler _mediatorHandler;

    /// <summary>
    /// Initializes a new instance of UsersController
    /// </summary>
    /// <param name="mediatorHandler">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CatalogsController(IMapper mapper, IProductService productService, IMediatorHandler mediatorHandler)
    {
        _mapper = mapper;
        _productService = productService;
        _mediatorHandler = mediatorHandler;
    }

    /// <summary>
    /// Retrieves a product by ID
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
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

    /// <summary>
    /// Retrieves products
    /// </summary>    
    /// <returns>The products if found</returns>
    [HttpGet("products")]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<ProductResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProducts()
    {
        var response = await _productService.GetAll();
        if(response == null || !response.Any())
            return NotFound("No products found.");

        return Ok(new ApiResponseWithData<IEnumerable<ProductResponse>>
        {
            Success = true,
            Message = "Products retrieved successfully",
            Data = response
        });
    }

}
