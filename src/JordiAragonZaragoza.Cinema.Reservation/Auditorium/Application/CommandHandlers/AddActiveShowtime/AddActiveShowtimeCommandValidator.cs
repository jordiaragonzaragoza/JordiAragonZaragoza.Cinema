namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.AddActiveShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;

    public sealed class AddActiveShowtimeCommandValidator : AbstractValidator<AddActiveShowtimeCommand>
    {
        public AddActiveShowtimeCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}