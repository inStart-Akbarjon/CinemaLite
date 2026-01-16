using CinemaLite.Application.CQRS.Ticket.Command.CreateTicket;
using FluentValidation;

namespace CinemaLite.Application.CQRS.Ticket.Validators;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(request => request.MovieId)
            .NotEmpty()
            .WithMessage("MovieId is required");
        
        RuleFor(request => request.SessionId)
            .NotEmpty()
            .WithMessage("SessionId is required");
    }
}