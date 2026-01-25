namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.PurchaseReservation
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;

    public sealed class PurchaseReservationCommandValidator : AbstractValidator<PurchaseReservationCommand>
    {
        public PurchaseReservationCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
             .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.ReservationId)
             .NotEmpty().WithMessage("ReservationId is required.");
        }
    }
}