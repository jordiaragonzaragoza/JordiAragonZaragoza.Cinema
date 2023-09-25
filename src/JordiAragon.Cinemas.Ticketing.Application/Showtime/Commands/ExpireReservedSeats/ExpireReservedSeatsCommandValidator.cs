namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.Commands.ExpireReservedSeats
{
    using FluentValidation;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Commands;

    public class ExpireReservedSeatsCommandValidator : AbstractValidator<ExpireReservedSeatsCommand>
    {
        public ExpireReservedSeatsCommandValidator()
        {
            this.RuleFor(x => x.TicketId)
              .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}