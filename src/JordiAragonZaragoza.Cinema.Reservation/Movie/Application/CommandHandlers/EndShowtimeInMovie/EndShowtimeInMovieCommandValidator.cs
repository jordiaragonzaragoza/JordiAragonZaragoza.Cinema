namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.EndShowtimeInMovie
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;

    public sealed class EndShowtimeInMovieCommandValidator : AbstractValidator<EndShowtimeInMovieCommand>
    {
        public EndShowtimeInMovieCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}