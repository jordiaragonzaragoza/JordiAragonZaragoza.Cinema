namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.RemoveActiveShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;

    public sealed class RemoveActiveShowtimeCommandValidator : AbstractValidator<RemoveActiveShowtimeCommand>
    {
        public RemoveActiveShowtimeCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}