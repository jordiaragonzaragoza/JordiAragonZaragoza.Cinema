namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.CreateAuditorium
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;

    public sealed class CreateAuditoriumCommandValidator : AbstractValidator<CreateAuditoriumCommand>
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