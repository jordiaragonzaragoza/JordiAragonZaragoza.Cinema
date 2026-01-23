namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ReserveSeats
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;

    public sealed class ReserveSeatsCommandValidator : AbstractValidator<ReserveSeatsCommand>
    {
        public ReserveSeatsCommandValidator()
        {
            this.RuleFor(x => x.ReservationId)
                .NotEmpty().WithMessage("ReservationId is required.");

            this.RuleFor(x => x.ShowtimeId)
                .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            this.RuleFor(x => x.SeatsIds)
                .NotEmpty().WithMessage("SeatsIds is required.");
        }
    }
}