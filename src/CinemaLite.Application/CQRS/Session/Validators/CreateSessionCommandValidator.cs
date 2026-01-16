using CinemaLite.Application.CQRS.Session.Commands.CreateSession;
using FluentValidation;

namespace CinemaLite.Application.CQRS.Session.Validators;

public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator()
    {
        RuleFor(s => s.MovieId)
            .NotEmpty()
            .WithMessage("Movie id cannot be empty");
        
        RuleFor(s => s.CinemaName)
            .NotEmpty()
            .WithMessage("Property CinemaName cannot be empty");
        
        RuleFor(s => s.Price)
            .NotEmpty()
            .WithMessage("Price cannot be empty")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
        
        RuleFor(s => s.AvailableSeats)
            .NotEmpty()
            .WithMessage("AvailableSeats cannot be empty")
            .GreaterThan(0)
            .WithMessage("AvailableSeats must be greater than 0");

        RuleFor(s => s.StartTime)
            .NotEmpty()
            .WithMessage("Start time cannot be empty");
    }
}