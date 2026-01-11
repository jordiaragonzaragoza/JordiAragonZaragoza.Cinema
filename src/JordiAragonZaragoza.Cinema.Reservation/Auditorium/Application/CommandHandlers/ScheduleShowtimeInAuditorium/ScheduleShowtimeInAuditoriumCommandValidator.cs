namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.ScheduleShowtimeInAuditorium
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;

    public sealed class ScheduleShowtimeInAuditoriumCommandValidator : AbstractValidator<ScheduleShowtimeInAuditoriumCommand>
    {
        public ScheduleShowtimeInAuditoriumCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}