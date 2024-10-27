namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.EndShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class EndShowtimeCommandValidator : BaseValidator<EndShowtimeCommand>
    {
        public EndShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}