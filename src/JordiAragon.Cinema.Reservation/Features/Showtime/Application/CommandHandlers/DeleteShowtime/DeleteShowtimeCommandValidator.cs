namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.CreateShowtime
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public class DeleteShowtimeCommandValidator : BaseValidator<DeleteShowtimeCommand>
    {
        public DeleteShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}