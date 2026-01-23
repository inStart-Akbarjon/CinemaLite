using CinemaLite.Application.CQRS.Session.Commands.UpdateSession;
using CinemaLite.Application.DTOs.Session.Request;
using FluentValidation;

namespace CinemaLite.Application.CQRS.Session.Validators;

public class UpdateSessionCommandValidator : AbstractValidator<UpdateSessionRequest>
{
    public UpdateSessionCommandValidator()
    {
        RuleFor(s => s.CinemaName)
            .NotEmpty()
            .WithMessage("Property CinemaName cannot be empty");
        
        RuleFor(s => s.Price)
            .NotEmpty()
            .WithMessage("Price cannot be empty")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
        
        RuleFor(s => s.SeatsPerRow)
            .NotEmpty()
            .WithMessage("SeatsPerRow cannot be empty")
            .GreaterThan(0)
            .WithMessage("SeatsPerRow must be greater than 0");
        
        RuleFor(s => s.TotalRows)
            .NotEmpty()
            .WithMessage("TotalRows cannot be empty")
            .GreaterThan(0)
            .WithMessage("TotalRows must be greater than 0");

        RuleFor(s => s.StartTime)
            .NotEmpty()
            .WithMessage("Start time cannot be empty");
    }
}