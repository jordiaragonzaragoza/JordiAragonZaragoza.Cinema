namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.CancelShowtimeInMovie
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;

    public sealed class CancelShowtimeInMovieCommandValidator : AbstractValidator<CancelShowtimeInMovieCommand>
    {
        public CancelShowtimeInMovieCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}