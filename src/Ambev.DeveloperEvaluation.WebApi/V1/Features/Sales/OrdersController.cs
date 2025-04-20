using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvoluation.Security.Extensions;
using Ambev.DeveloperEvoluation.Security.Services.AspNetUser;

namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Sales;

/// <summary>
/// Controller for managing orders operations
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class OrdersController : BaseController
{    
    private readonly IOrderQueries _orderQueries;
    private readonly IAspNetUser _aspNetUser;

    /// <summary>
    /// Initializes a new instance of OrdersController
    /// </summary>
    /// <param name="orderQueries">The orderQueries instance</param>    
    public OrdersController(IOrderQueries orderQueries, IAspNetUser aspNetUser)
    {
        _orderQueries = orderQueries;
        _aspNetUser = aspNetUser;
    }

    /// <summary>
    /// Retrieves orders
    /// </summary>    
    /// <returns>The orders if found</returns>
    [ClaimsAuthorize("Permissions", "Read")]        
    [HttpGet("my-orders")]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<OrderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrders()
    {
        var response = await _orderQueries.GetCustomerOrders(_aspNetUser.GetUserId());
        if (response == null || !response.Any())
            return NotFound("Orders not found.");

        return Ok(new ApiResponseWithData<IEnumerable<OrderResponse>>
        {
            Success = true,
            Message = "Orders retrieved successfully",
            Data = response
        });
    }

}
