namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime
{
    using System;
    using Ardalis.GuardClauses;
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class ScheduleShowtimeCommandValidator : BaseValidator<ScheduleShowtimeCommand>
    {
        private readonly IDateTime dateTime;

        public ScheduleShowtimeCommandValidator(IDateTime dateTime)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));

            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.SessionDateOnUtc)
                .Must(this.ValidateExpirationDateOnUtc).WithMessage("Session Date must be a future date.");
        }

        private bool ValidateExpirationDateOnUtc(DateTimeOffset sessionDateOnUtc)
        {
            if (sessionDateOnUtc < this.dateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}