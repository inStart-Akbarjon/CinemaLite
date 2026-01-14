using CinemaLite.Application.CQRS.Auth.Login.Commands;
using FluentValidation;

namespace CinemaLite.Application.CQRS.Auth.Login.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Invalid email address.")
            .NotEmpty()
            .WithMessage("Email is required.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}