using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Auth.AuthenticateUserFeature;
public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(x => x.ConfirmePassword)
            .NotEmpty()
            .WithMessage("ConfirmePassword is required")
            .Equal(x => x.Password)
            .WithMessage("ConfirmePassword must match Password");

        RuleFor(x => x.FistName)
            .NotEmpty()
            .WithMessage("FistName is required");           

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName is required");

        RuleFor(x => x.UserName)
            .Empty()
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("UserName must be valid when provided");

        RuleFor(x => x.Phone)
            .Matches(@"^\+?55\s?\(?\d{2}\)?\s?\d{4,5}-\d{4}$")
            .WithMessage("Phone must be a valid Brazilian number in the format +55 (XX) XXXXX-XXXX or +55 (XX) XXXX-XXXX")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

    }
}
