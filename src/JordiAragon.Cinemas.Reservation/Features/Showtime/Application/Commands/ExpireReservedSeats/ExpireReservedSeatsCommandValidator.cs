namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Commands.ExpireReservedSeats
{
    using FluentValidation;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Commands;

    public class ExpireReservedSeatsCommandValidator : AbstractValidator<ExpireReservedSeatsCommand>
    {
        public ExpireReservedSeatsCommandValidator()
        {
            this.RuleFor(x => x.TicketId)
              .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}