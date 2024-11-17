namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.RemoveAuditorium
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class RemoveAuditoriumCommandValidator : BaseValidator<RemoveAuditoriumCommand>
    {
        public RemoveAuditoriumCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");
        }
    }
}