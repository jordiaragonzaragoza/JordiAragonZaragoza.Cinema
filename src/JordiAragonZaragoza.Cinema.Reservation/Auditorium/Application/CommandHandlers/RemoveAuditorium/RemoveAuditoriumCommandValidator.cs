namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.RemoveAuditorium
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;

    public sealed class RemoveAuditoriumCommandValidator : AbstractValidator<RemoveAuditoriumCommand>
    {
        public RemoveAuditoriumCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");
        }
    }
}