namespace JordiAragon.Cinema.Reservation.Movie.Application.CommandHanders.AddMovie
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class RemoveMovieCommandValidator : BaseValidator<RemoveMovieCommand>
    {
        public RemoveMovieCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");
        }
    }
}