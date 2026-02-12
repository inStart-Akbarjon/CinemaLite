using CinemaLite.Application.CQRS.Movie.Commands.UpdateMovie;
using CinemaLite.Application.DTOs.Movie.Request;
using FluentValidation;

namespace CinemaLite.Application.CQRS.Movie.Validators;

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(m => m.Title)
            .NotEmpty()
            .WithMessage("Title is required");
        
        RuleFor(movie => movie.DurationMinutes)
            .NotEmpty()
            .WithMessage("DurationMinutes is required")
            .GreaterThan(0)
            .WithMessage("DurationMinutes must be greater than zero");

        RuleFor(movie => movie.Genre)
            .NotEmpty()
            .WithMessage("Genre is required");
        
        RuleFor(x => x.TopSubscriptionPeriod)
            .GreaterThan(0)
            .When(x => x.IsTop)
            .WithMessage("TopSubscriptionPeriod must be greater than 0 when IsTop is true.");

        RuleFor(x => x.TopSubscriptionPeriod)
            .Equal(0)
            .When(x => !x.IsTop)
            .WithMessage("TopSubscriptionPeriod must be 0 when IsTop is false.");
    }
}