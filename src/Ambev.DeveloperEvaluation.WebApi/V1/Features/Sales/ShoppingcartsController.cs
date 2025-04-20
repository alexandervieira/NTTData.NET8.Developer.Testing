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
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvoluation.Security.Services.AspNetUser;
using Ambev.DeveloperEvoluation.Security.Extensions;

namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Sales;

/// <summary>
/// Controller for managing shoppingcarts operations
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/carts")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ShoppingcartsController : BaseController
{
    private readonly IOrderQueries _orderQueries;
    private readonly IProductService _productService;
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IAspNetUser _AspNetUser;

    public ShoppingcartsController(IOrderQueries orderQueries, IProductService productService, 
                                   IMediatorHandler mediatorHandler, IAspNetUser aspNetUser)
    {
        _orderQueries = orderQueries;
        _productService = productService;
        _mediatorHandler = mediatorHandler;
        _AspNetUser = aspNetUser;
    }

    /// <summary>
    /// Initializes a new instance of ShoppingcartsController 
    /// </summary>
    /// <param name="orderQueries">The orderQueries instance</param>    
    /// <param name="productService">The productService instance</param>  
    /// <param name="mediatorHandler">The mediatorHandler instance</param>  

    [ClaimsAuthorize("Permissions", "Read")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCarts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8, [FromQuery] string? query = null)
    {
        var response = await _orderQueries.GetAll(pageNumber, pageSize, query);

        if (response == null || !response.Any())
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Cart not found."
            });

        return Ok(new PaginatedResponse<CartResponse>
        {
            Success = true,
            Message = "Carts retrieved successfully",
            Data = response,
            CurrentPage = response.CurrentPage,
            TotalPages = response.TotalPages,
            TotalCount = response.TotalCount
        });
    }

    /// <summary>
    /// Retrieves shoppingcarts
    /// </summary>    
    /// <returns>The shoppingcarts if found</returns>
    [ClaimsAuthorize("Permissions", "Read")]
    [HttpGet("my-cart/{customerId}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerCart([FromRoute] Guid customerId)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });
        
        if (customerId != _AspNetUser.GetUserId())
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Customer ID does not match the authenticated user."
            });
        }

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

    [ClaimsAuthorize("Permissions", "Write")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<AddUpdateOrderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItems([FromBody] AddUpdateOrderRequest request, CancellationToken cancellationToken)
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

        var validator = new AddUpdateOrderRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new AddOrderItemsCommand(request.CustomerId, request.Items);

        var result = await _mediatorHandler.SendCommand(command);
        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to add item to cart."
            });
        }

        var cartResponse = await _orderQueries.GetCustomerCart(request.CustomerId);
        if (cartResponse == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Cart not found."
            });

        cartResponse.CustomerId = request.CustomerId;
        cartResponse.Items = request.Items.Select(item => new CartItemResponse
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity

        }).ToList();

        var OrderResponse = new AddUpdateOrderResponse
        {
            CustomerId = cartResponse.CustomerId,
            OrderId = cartResponse.OrderId,
            Items = cartResponse.Items.Select(item => new AddUpdateOrderItemsResponse
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList(),
        };

        return Created(string.Empty, new ApiResponseWithData<AddUpdateOrderResponse>
        {
            Success = true,
            Message = "Item added successfully",
            Data = OrderResponse
        });

    }

    [ClaimsAuthorize("Permissions", "Edit")]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponseWithData<AddUpdateOrderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItems([FromBody] AddUpdateOrderRequest request, CancellationToken cancellationToken)
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

        var validator = new AddUpdateOrderRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = new UpdateOrderItemsCommand(request.CustomerId, request.Items);

        var result = await _mediatorHandler.SendCommand(command);
        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to add item to cart."
            });
        }

        var cartResponse = await _orderQueries.GetCustomerCart(request.CustomerId);
        if (cartResponse == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Cart not found."
            });
        cartResponse.CustomerId = request.CustomerId;
        cartResponse.Items = request.Items.Select(item => new CartItemResponse
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity

        }).ToList();

        var OrderResponse = new AddUpdateOrderResponse
        {
            CustomerId = cartResponse.CustomerId,
            OrderId = cartResponse.OrderId,
            Items = cartResponse.Items.Select(item => new AddUpdateOrderItemsResponse
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList(),
        };

        return Created(string.Empty, new ApiResponseWithData<AddUpdateOrderResponse>
        {
            Success = true,
            Message = "Item added successfully",
            Data = OrderResponse
        });

    }

    [ClaimsAuthorize("Permissions", "Write")]
    [HttpPost("{productId}/add-item/{quantity}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem([FromRoute] Guid productId, [FromRoute] int quantity, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        var request = new ItemRequest(productId, quantity);

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

        var command = new AddOrderItemCommand(_AspNetUser.GetUserId(), productResponse.Id,
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

    [ClaimsAuthorize("Permissions", "Edit")]
    [HttpPut("{productId}/update-item/{quantity}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromRoute] Guid productId, [FromRoute] int quantity, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });
        var request = new ItemRequest(productId, quantity);

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

        var response = await _productService.GetById(productId);

        if (response == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = $"Product with ID {productId} not found."
            });

        var command = new UpdateOrderItemCommand(_AspNetUser.GetUserId(), response.Id, request.Quantity);
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

    [ClaimsAuthorize("Permissions", "Delete")]
    [HttpDelete("debit-units")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrderItemUnits([FromBody] DeleteOrderItemUnitsRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        var validator = new DeleteOrderItemUnitsValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        if (request == null)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Request body cannot be null."
            });

        var command = new DeleteOrderItemUnitsCommand(request.CustomerId, request.ProductId, request.Quantity);

        var result = await _mediatorHandler.SendCommand(command);

        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to delete units from cart."
            });
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Units deleted successfully"
        });
    }

    [ClaimsAuthorize("Permissions", "Delete")]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

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
        var command = new DeleteOrderItemCommand(_AspNetUser.GetUserId(), id);
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

    [ClaimsAuthorize("Permissions", "Write")]
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

        var command = new ApplyVoucherToOrderCommand(_AspNetUser.GetUserId(), request.VoucherCode);
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

    [ClaimsAuthorize("Permissions", "Write")]
    [ClaimsAuthorize("Role", "Admin")]
    [HttpPost("generate-voucher")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateVoucher([FromBody] GenerateVoucherRequest request, CancellationToken cancellationToken)
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

        var command = new GenereateVoucherOrderCommand(request.Code, request.Percentage, request.DiscountValue,
            request.Quantity, request.DiscountVoucherType, request.UsageDate, request.ExpirationDate, request.Active, request.Used);

        var result = await _mediatorHandler.SendCommand(command);
        if (!result)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "fail to generate voucher."
            });
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Voucher generated successfully"
        });

    }

    [ClaimsAuthorize("Permissions", "Read")]
    [HttpGet("purchase-summary/{customerId}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PurchaseSummary([FromRoute] Guid customerId)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        if (customerId != _AspNetUser.GetUserId())
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Customer ID does not match the authenticated user."
            });
        }

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

    [ClaimsAuthorize("Permissions", "Write")]
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

        var cartResponse = await _orderQueries.GetCustomerCart(request.CustomerId);
        if (cartResponse == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Cart not found."
            });

        var command = new StartOrderCommand(cartResponse.OrderId, _AspNetUser.GetUserId(), cartResponse.TotalValue,
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
            Message = "start order successfully"
        });

    }

}
