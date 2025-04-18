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

        //RuleFor(x => x.Cpf)
        //    .NotEmpty()
        //    .WithMessage("Cpf is required")
        //    .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")
        //    .WithMessage("Invalid CPF format. Expected format: XXX.XXX.XXX-XX");





    }
}
