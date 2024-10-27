namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHanders.AddMovie
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class RemoveMovieCommandValidator : BaseValidator<RemoveMovieCommand>
    {
        public RemoveMovieCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");
        }
    }
}