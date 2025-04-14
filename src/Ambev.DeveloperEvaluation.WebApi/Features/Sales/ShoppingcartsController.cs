using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;
using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Application.Catalog.Validations;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.Commands;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Catalog;

/// <summary>
/// Controller for managing shoppingcarts operations
/// </summary>
[ApiController]
[Route("api/carts")]
public class ShoppingcartsController : BaseController
{    
    private readonly IOrderQueries _orderQueries;
    private readonly IProductService _productService;
    private readonly IMediatorHandler _mediatorHandler;
    private Guid GetCurrentUserIdTeste() => Guid.Parse("dfa87386-9c36-4e39-9f54-e1cf6e84a099");

    /// <summary>
    /// Initializes a new instance of ShoppingcartsController 
    /// </summary>
    /// <param name="orderQueries">The orderQueries instance</param>    
    /// <param name="productService">The productService instance</param>  
    /// <param name="mediatorHandler">The mediatorHandler instance</param>  
    public ShoppingcartsController(IOrderQueries orderQueries, IProductService productService, IMediatorHandler mediatorHandler)
    {
        _orderQueries = orderQueries;
        _productService = productService;
        _mediatorHandler = mediatorHandler;
    }

    /// <summary>
    /// Retrieves shoppingcarts
    /// </summary>    
    /// <returns>The shoppingcarts if found</returns>
    [HttpGet("my-cart/{customerId}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerCart([FromRoute] Guid customerId)
    {
        var response = await _orderQueries.GetCustomerCart(customerId);
        if (response == null)
            return NotFound("Cart not found.");

        return Ok(new ApiResponseWithData<CartResponse>
        {
            Success = true,
            Message = "Cart retrieved successfully",
            Data = response
        });
    }

    [HttpPost("{id}/add-item/{quantity}")]
    [ProducesResponseType(typeof(ApiResponseWithData<ProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem([FromRoute]Guid id, [FromRoute] int quantity , CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        var request = new AddItemRequest(id, quantity);

        var validator = new AddItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var productResponse = await _productService.GetById(request.ProductId);

        if (productResponse == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product not found."
            });

        if (productResponse.QuantityStock < request.Quantity)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"Insufficient stock for item"
            });
        }

        var command = new AddOrderItemCommand(GetCurrentUserIdTeste(), productResponse.Id, 
                            productResponse.Title, request.Quantity, productResponse.Price);
        
        var result = await _mediatorHandler.SendCommand(command);
        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to add item to cart."
            });
        }

        return Created(string.Empty, new ApiResponseWithData<ProductResponse>
        {
            Success = true,
            Message = "Item added successfully"           
        });
    }

}
