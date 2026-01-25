namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.CancelShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;

    public sealed class CancelShowtimeCommandValidator : AbstractValidator<CancelShowtimeCommand>
    {
        public CancelShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}