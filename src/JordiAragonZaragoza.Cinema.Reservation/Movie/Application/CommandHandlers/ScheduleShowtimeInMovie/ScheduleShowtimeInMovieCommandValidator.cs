namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.ScheduleShowtimeInMovie
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;

    public sealed class ScheduleShowtimeInMovieCommandValidator : AbstractValidator<ScheduleShowtimeInMovieCommand>
    {
        public ScheduleShowtimeInMovieCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}