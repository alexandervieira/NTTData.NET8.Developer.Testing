using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Users.GetUser;

/// <summary>
/// Validator for GetUserRequest
/// </summary>
public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    /// <summary>
    /// Initializes validation rules for GetUserRequest
    /// </summary>
    public GetUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
