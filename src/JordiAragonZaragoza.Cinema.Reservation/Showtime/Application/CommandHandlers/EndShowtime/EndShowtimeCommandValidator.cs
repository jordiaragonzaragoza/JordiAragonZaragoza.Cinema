namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.EndShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;

    public sealed class EndShowtimeCommandValidator : AbstractValidator<EndShowtimeCommand>
    {
        public EndShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}