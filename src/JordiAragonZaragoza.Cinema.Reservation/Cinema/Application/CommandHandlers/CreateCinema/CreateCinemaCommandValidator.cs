namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.CommandHandlers.AddCinema
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Commands;

    public sealed class CreateCinemaCommandValidator : AbstractValidator<CreateCinemaCommand>
    {
        public CreateCinemaCommandValidator()
        {
            this.RuleFor(x => x.CinemaId)
              .NotEmpty().WithMessage("CinemaId is required.");
        }
    }
}