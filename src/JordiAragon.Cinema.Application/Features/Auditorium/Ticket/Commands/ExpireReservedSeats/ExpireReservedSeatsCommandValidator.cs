namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.Commands.ExpireReservedSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands;

    public class ExpireReservedSeatsCommandValidator : AbstractValidator<ExpireReservedSeatsCommand>
    {
        public ExpireReservedSeatsCommandValidator()
        {
            this.RuleFor(x => x.TicketId)
              .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}