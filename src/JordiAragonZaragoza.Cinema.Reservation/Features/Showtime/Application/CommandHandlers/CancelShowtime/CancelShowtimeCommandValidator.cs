namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.CancelShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class CancelShowtimeCommandValidator : BaseValidator<CancelShowtimeCommand>
    {
        public CancelShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}