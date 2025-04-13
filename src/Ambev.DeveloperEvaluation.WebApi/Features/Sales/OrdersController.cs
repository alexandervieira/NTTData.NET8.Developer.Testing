using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Catalog;

/// <summary>
/// Controller for managing orders operations
/// </summary>
[ApiController]
[Route("api/orders")]
public class OrdersController : BaseController
{
    //private readonly IMediator _mediator;
    private readonly IOrderQueries _orderQueries;
    private Guid GetCurrentUserIdTeste() => Guid.Parse("dfa87386-9c36-4e39-9f54-e1cf6e84a099");

    /// <summary>
    /// Initializes a new instance of OrdersController
    /// </summary>
    /// <param name="orderQueries">The orderQueries instance</param>    
    public OrdersController(IOrderQueries orderQueries)
    {
        _orderQueries = orderQueries;
    }

    /// <summary>
    /// Retrieves orders
    /// </summary>    
    /// <returns>The orders if found</returns>
    [HttpGet("my-orders")]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<OrderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrders()
    {
        var response = await _orderQueries.GetCustomerOrders(GetCurrentUserIdTeste());
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
