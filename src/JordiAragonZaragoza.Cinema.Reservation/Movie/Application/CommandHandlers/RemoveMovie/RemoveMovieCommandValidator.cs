namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.AddMovie
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;

    public sealed class RemoveMovieCommandValidator : AbstractValidator<RemoveMovieCommand>
    {
        public RemoveMovieCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");
        }
    }
}