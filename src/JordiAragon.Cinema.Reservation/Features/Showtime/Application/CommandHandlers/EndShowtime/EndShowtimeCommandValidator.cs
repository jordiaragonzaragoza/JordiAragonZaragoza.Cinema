namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.EndShowtime
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class EndShowtimeCommandValidator : BaseValidator<EndShowtimeCommand>
    {
        public EndShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}