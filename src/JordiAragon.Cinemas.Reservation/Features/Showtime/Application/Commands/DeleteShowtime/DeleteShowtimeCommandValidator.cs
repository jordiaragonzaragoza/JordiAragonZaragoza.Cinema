namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Commands.CreateShowtime
{
    using FluentValidation;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Commands;

    public class DeleteShowtimeCommandValidator : AbstractValidator<DeleteShowtimeCommand>
    {
        public DeleteShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}