using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Application.Catalog.Validations;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ambev.DeveloperEvoluation.Security.Extensions;


namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Catalog.Admin;

/// <summary>
/// Controller for managing user operations
/// </summary>

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/admin")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdminProductsController : BaseController
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
    public AdminProductsController(IMapper mapper, IProductService productService,
                                   IMediatorHandler mediatorHandler)
    {
        _mapper = mapper;
        _productService = productService;
        _mediatorHandler = mediatorHandler;
    }

    /// <summary>
    /// Retrieves products
    /// </summary>    
    /// <returns>The products if found</returns>
    [ClaimsAuthorize("Permissions", "Read")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpGet("products")]
    [ProducesResponseType(typeof(PaginatedResponse<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8, [FromQuery] string? query = null)
    {
        var response = await _productService.GetAll(pageNumber, pageSize, query);

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
            CurrentPage = response.CurrentPage,
            TotalPages = response.TotalPages,
            TotalCount = response.TotalCount
        });
    }

    [ClaimsAuthorize("Permissions", "Read")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpGet("products/category/{category}")]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<ProductResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsByCategoryName([FromRoute] string category, CancellationToken cancellationToken)
    {
        var request = new GetProductsCategoryNameRequest { Name = category };
        var validator = new GetProductsCategoryNameValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await _productService.GetByCategoryName(request.Name);

        if (response == null || !response.Any())
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Failed to get products by category name."
            });

        return Ok(new ApiResponseWithData<IEnumerable<ProductResponse>>
        {
            Success = true,
            Message = "Product retrieved successfully",
            Data = response
        });
    }

    /// <summary>
    /// Retrieves a product by ID
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    [ClaimsAuthorize("Permissions", "Read")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpGet("products/{id}")]
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

        if (response == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {id} not found."
            });

        return Ok(new ApiResponseWithData<ProductResponse>
        {
            Success = true,
            Message = "Product retrieved successfully",
            Data = response
        });
    }

    [ClaimsAuthorize("Permissions", "Write")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpPost("products")]
    [ProducesResponseType(typeof(ApiResponseWithData<ProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        if (request == null)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Request body cannot be null."
            });

        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await _productService.AddProduct(request);

        if (response == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product not found."
            });

        return Created(string.Empty, new ApiResponseWithData<ProductResponse>
        {
            Success = true,
            Message = "Product created successfully",
            Data = response
        });
    }

    [ClaimsAuthorize("Permissions", "Edit")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpPut("products/{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "The ID in the route does not match the ID in the request body."
            });

        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        if (request == null)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Request body cannot be null."
            });

        var validator = new UpdateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await _productService.UpdateProduct(request);

        if (response == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {id} not found."
            });

        return Ok(new ApiResponseWithData<ProductResponse>
        {
            Success = true,
            Message = "Product updated successfully",
            Data = response
        });
    }

    [ClaimsAuthorize("Permissions", "Delete")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpDelete("products/{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteProductRequest { Id = id };
        var validator = new DeleteProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await _productService.DeleteProduct(id);

        if (!response)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {id} not found."
            });

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User deleted successfully"
        });
    }

    [ClaimsAuthorize("Permissions", "Read")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpGet("products/categories")]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<CategoryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategories()
    {
        var response = await _productService.GetCategories();
        if (response == null || !response.Any())
            return NotFound("Categories not found.");

        return Ok(new ApiResponseWithData<IEnumerable<CategoryResponse>>
        {
            Success = true,
            Message = "Categories retrieved successfully",
            Data = response
        });
    }

    [ClaimsAuthorize("Permissions", "Write")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpPost("products/add-category")]
    [ProducesResponseType(typeof(ApiResponseWithData<CategoryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        if (request == null)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Request body cannot be null."
            });

        var validator = new CategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await _productService.AddCategory(request);

        if (response == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Category not found."
            });

        return Created(string.Empty, new ApiResponseWithData<CategoryResponse>
        {
            Success = true,
            Message = "Category created successfully",
            Data = response
        });
    }

}
