namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.CreateShowtime
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public class EndShowtimeCommandValidator : BaseValidator<EndShowtimeCommand>
    {
        public EndShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}