using CinemaLite.Application.CQRS.Cart.Commands.AddToCart;
using FluentValidation;

namespace CinemaLite.Application.CQRS.Cart.Validators;

public class AddTicketToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddTicketToCartCommandValidator()
    {
        RuleFor(request => request.MovieId)
            .NotEmpty()
            .WithMessage("MovieId is required");
        
        RuleFor(request => request.SessionId)
            .NotEmpty()
            .WithMessage("SessionId is required");
    }
}