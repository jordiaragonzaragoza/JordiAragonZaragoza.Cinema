﻿namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.AddMovie
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class AddMovieCommandValidator : BaseValidator<AddMovieCommand>
    {
        public AddMovieCommandValidator()
        {
            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.Title)
              .NotEmpty().WithMessage("Title is required.");

            this.RuleFor(x => x.Runtime)
              .NotEmpty().WithMessage("Runtime is required.");

            this.RuleFor(x => x.StartingPeriod)
              .NotEmpty().WithMessage("StartingPeriod is required.");

            this.RuleFor(x => x.EndOfPeriod)
              .NotEmpty().WithMessage("EndOfPeriod is required.");
        }
    }
}