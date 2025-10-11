namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ExpireReservedSeats
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;

    public sealed class ExpireReservedSeatsCommandValidator : AbstractValidator<ExpireReservedSeatsCommand>
    {
        public ExpireReservedSeatsCommandValidator()
        {
            this.RuleFor(x => x.ReservationId)
              .NotEmpty().WithMessage("ReservationId is required.");
        }
    }
}