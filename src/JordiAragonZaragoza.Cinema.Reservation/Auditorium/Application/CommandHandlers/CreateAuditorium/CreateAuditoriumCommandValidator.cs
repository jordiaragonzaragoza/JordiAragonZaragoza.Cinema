namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.AddAuditorium
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class CreateAuditoriumCommandValidator : BaseValidator<CreateAuditoriumCommand>
    {
        public CreateAuditoriumCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Name is required.");

            this.RuleFor(x => x.Rows)
              .NotEmpty().WithMessage("Rows is required.");

            this.RuleFor(x => x.SeatsPerRow)
              .NotEmpty().WithMessage("SeatsPerRow is required.");
        }
    }
}