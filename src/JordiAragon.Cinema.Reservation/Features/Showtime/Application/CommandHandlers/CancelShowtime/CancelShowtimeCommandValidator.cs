namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.CancelShowtime
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class CancelShowtimeCommandValidator : BaseValidator<CancelShowtimeCommand>
    {
        public CancelShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}