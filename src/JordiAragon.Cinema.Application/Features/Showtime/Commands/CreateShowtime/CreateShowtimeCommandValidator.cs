namespace JordiAragon.Cinema.Application.Features.Showtime.Commands.CreateShowtime
{
    using System;
    using Ardalis.GuardClauses;
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class CreateShowtimeCommandValidator : AbstractValidator<CreateShowtimeCommand>
    {
        private readonly IDateTime dateTime;

        public CreateShowtimeCommandValidator(IDateTime dateTime)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));

            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.SessionDateOnUtc)
                .Must(this.ValidateExpirationDateOnUtc).WithMessage("Session Date must be a future date.");
        }

        private bool ValidateExpirationDateOnUtc(DateTime sessionDateOnUtc)
        {
            if (sessionDateOnUtc < this.dateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}