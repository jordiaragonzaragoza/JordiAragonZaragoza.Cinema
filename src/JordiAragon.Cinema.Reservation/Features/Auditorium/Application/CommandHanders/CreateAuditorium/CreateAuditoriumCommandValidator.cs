namespace JordiAragon.Cinema.Reservation.Auditorium.Application.CommandHanders.AddAuditorium
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

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