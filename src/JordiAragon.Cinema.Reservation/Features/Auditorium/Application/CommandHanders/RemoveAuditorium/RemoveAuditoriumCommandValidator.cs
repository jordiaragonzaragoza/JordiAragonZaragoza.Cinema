namespace JordiAragon.Cinema.Reservation.Auditorium.Application.CommandHanders.RemoveAuditorium
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class RemoveAuditoriumCommandValidator : BaseValidator<RemoveAuditoriumCommand>
    {
        public RemoveAuditoriumCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");
        }
    }
}