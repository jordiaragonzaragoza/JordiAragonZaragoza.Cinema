namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.AddActiveShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;

    public sealed class AddActiveShowtimeCommandValidator : AbstractValidator<AddActiveShowtimeCommand>
    {
        public AddActiveShowtimeCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}