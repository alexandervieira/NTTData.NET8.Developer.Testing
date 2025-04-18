using Ambev.DeveloperEvoluation.Security.Models;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

/// <summary>
/// Command for authenticating a user in the system.
/// Implements IRequest for mediator pattern handling.
/// </summary>
public class RegisterUserCommand : IRequest<UserResponseLogin>
{
    public string FistName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? UserName { get; set; } = null!;
    public string? Cpf { get; set; }
    public string Email { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public bool Active { get; set; }
    public string Password { get; set; } = null!;
    public string ConfirmePassword { get; set; } = null!;
}
