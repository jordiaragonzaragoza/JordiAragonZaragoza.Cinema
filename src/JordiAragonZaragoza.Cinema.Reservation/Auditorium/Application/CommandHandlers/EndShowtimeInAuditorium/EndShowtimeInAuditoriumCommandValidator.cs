namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.EndShowtimeInAuditorium
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;

    public sealed class EndShowtimeInAuditoriumCommandValidator : AbstractValidator<EndShowtimeInAuditoriumCommand>
    {
        public EndShowtimeInAuditoriumCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}