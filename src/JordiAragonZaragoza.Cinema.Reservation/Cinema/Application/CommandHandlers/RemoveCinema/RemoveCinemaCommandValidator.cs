namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.CommandHandlers.RemoveCinema
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Commands;

    public sealed class RemoveCinemaCommandValidator : AbstractValidator<RemoveCinemaCommand>
    {
        public RemoveCinemaCommandValidator()
        {
            this.RuleFor(x => x.CinemaId)
              .NotEmpty().WithMessage("CinemaId is required.");
        }
    }
}