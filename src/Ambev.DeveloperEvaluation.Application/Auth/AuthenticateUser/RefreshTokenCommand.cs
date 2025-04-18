using Ambev.DeveloperEvoluation.Security.Models;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
public class RefreshTokenCommand : IRequest<UserResponseLogin>
{    
    public string RefreshToken { get; set; } = string.Empty;
    
}
