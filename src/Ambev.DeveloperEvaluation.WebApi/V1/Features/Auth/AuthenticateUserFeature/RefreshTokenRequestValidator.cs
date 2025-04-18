using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Auth.AuthenticateUserFeature;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.")
            .Matches(@"^[a-zA-Z0-9\-_.]+$")
            .WithMessage("Refresh token can only contain letters, numbers, dashes, underscores, and periods.");

    }
}
