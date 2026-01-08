namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.RemoveActiveShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;

    public sealed class RemoveActiveShowtimeCommandValidator : AbstractValidator<RemoveActiveShowtimeCommand>
    {
        public RemoveActiveShowtimeCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}