﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Commands.ReserveSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;

    public class ReserveSeatsCommandValidator : AbstractValidator<ReserveSeatsCommand>
    {
        public ReserveSeatsCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
                .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.SeatsIds)
                .NotEmpty().WithMessage("SeatsIds is required.");
        }
    }
}