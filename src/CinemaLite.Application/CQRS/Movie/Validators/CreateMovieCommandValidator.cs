using CinemaLite.Application.CQRS.Movie.Commands.CreateMovie;
using FluentValidation;

namespace CinemaLite.Application.CQRS.Movie.Validators;

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
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
    }
}