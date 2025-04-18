using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvoluation.Security.Models;
using Ambev.DeveloperEvaluation.WebApi.V1.Features.Auth.AuthenticateUserFeature;
using Asp.Versioning;

namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Auth;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of AuthController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Authenticates a user with their credentials
    /// </summary>
    /// <param name="request">The authentication request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication token if successful</returns>
    [HttpPost("signin")]
    [ProducesResponseType(typeof(ApiResponseWithData<UserResponseLogin>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        var validator = new AuthenticateUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<AuthenticateUserCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UserResponseLogin>
        {
            Success = true,
            Message = "User authenticated successfully",
            Data = response
        });
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(ApiResponseWithData<UserResponseLogin>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        var validator = new RegisterUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<RegisterUserCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UserResponseLogin>
        {
            Success = true,
            Message = "User authenticated successfully",
            Data = response
        });
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponseWithData<UserResponseLogin>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Model state is invalid."
            });

        var validator = new RefreshTokenRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<RefreshTokenCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UserResponseLogin>
        {
            Success = true,
            Message = "Refresh Token successfully",
            Data = response
        });
    }

    [HttpPost("logout")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {

        var command = new LogoutCommand { LoggedOut = true };
        var response = await _mediator.Send(command);
        if (!response)
            return BadRequest(new ApiResponse
            {
                Success = response,
                Message = "Logout failed"
            });

        return Ok(new ApiResponse
        {
            Success = response,
            Message = "Logout success"
        });
    }
}
