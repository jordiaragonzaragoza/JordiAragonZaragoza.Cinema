namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime
{
    using System;
    using Ardalis.GuardClauses;
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class ScheduleShowtimeCommandValidator : BaseValidator<ScheduleShowtimeCommand>
    {
        private readonly IDateTime dateTime;

        public ScheduleShowtimeCommandValidator(IDateTime dateTime)
        {
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.MovieId)
              .NotEmpty().WithMessage("MovieId is required.");

            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.SessionDateOnUtc)
                .Must(this.ValidateExpirationDateOnUtc).WithMessage("Session Date must be a future date.");
        }

        // TODO: Move to domain.
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