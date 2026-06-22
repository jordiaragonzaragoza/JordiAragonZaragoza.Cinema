namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.CancelReservation
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;

    public sealed class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
    {
        public CancelReservationCommandValidator()
        {
            this.RuleFor(x => x.ReservationId)
              .NotEmpty().WithMessage("ReservationId is required.");
        }
    }
}