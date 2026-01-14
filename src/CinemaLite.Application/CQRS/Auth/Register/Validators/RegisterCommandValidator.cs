using CinemaLite.Application.CQRS.Auth.Register.Commands;
using FluentValidation;

namespace CinemaLite.Application.CQRS.Auth.Register.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Invalid email address.")
            .NotEmpty()
            .WithMessage("Email is required.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Must(password => password.Length > 6)
            .WithMessage("Password should be more than 6 characters.");
    }
}