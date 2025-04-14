using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;
using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvaluation.Application.Catalog.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Application.Catalog.Validations;

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
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem([FromRoute] Guid id, [FromRoute] int quantity, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        var request = new ItemRequest(id, quantity);

        var validator = new ItemRequestValidator();
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

        return Created(string.Empty, new ApiResponse
        {
            Success = true,
            Message = "Item added successfully"
        });
    }

    [HttpPut("{id}/update-item/{quantity}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromRoute] Guid id, [FromRoute] int quantity, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });
        var request = new ItemRequest(id, quantity);

        if (request == null)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Request body cannot be null."
            });

        var validator = new ItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await _productService.GetById(id);

        if (response == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {id} not found."
            });

        var command = new UpdateOrderItemCommand(GetCurrentUserIdTeste(), response.Id, request.Quantity);
        var result = await _mediatorHandler.SendCommand(command);
        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to update item in cart."
            });
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Item updated successfully"

        });
    }

    [HttpDelete("delete-item/{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteProductRequest { Id = id };
        var validator = new DeleteProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await _productService.GetById(id);
        if (response == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {id} not found."
            });
        var command = new RemoveOrderItemCommand(GetCurrentUserIdTeste(), id);
        var result = await _mediatorHandler.SendCommand(command);

        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to delete item from cart."
            });
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Item deleted successfully"
        });
    }

    [HttpPost("apply-voucher/{voucher}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApplyVoucher([FromRoute] string voucher, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        var request = new VoucherRequest(voucher);

        var validator = new VoucherRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new ApplyVoucherToOrderCommand(GetCurrentUserIdTeste(), request.VoucherCode);
        var result = await _mediatorHandler.SendCommand(command);
        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to apply voucher."
            });
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Voucher applied successfully"
        });

    }

    [HttpGet("purchase-summary/{customerId}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PurchaseSummary([FromRoute] Guid customerId)
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

    [HttpPost("start-order")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartOrder([FromBody] CartRequest request, CancellationToken cancellationToken)
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

        var validator = new CartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var cartResponse = await _orderQueries.GetCustomerCart(GetCurrentUserIdTeste());
        if (cartResponse == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Cart not found."
            });

        var command = new StartOrderCommand(cartResponse.OrderId, GetCurrentUserIdTeste(), cartResponse.TotalValue, 
            request.Payment.CardName, request.Payment.CardNumber, request.Payment.CardExpiration, request.Payment.CardCvv);
        
        var result = await _mediatorHandler.SendCommand(command);

        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to start order."
            });
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Voucher applied successfully"
        });

    }

}
