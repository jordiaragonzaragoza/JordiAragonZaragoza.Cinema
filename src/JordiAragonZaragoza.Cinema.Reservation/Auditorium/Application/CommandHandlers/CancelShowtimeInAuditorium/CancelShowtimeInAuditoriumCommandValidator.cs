namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.CancelShowtimeInAuditorium
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;

    public sealed class CancelShowtimeInAuditoriumCommandValidator : AbstractValidator<CancelShowtimeInAuditoriumCommand>
    {
        public CancelShowtimeInAuditoriumCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}