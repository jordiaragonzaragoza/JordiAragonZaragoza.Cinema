namespace JordiAragon.Cinema.Application.Features.Showtime.Commands.ReserveSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;

    public class ReserveSeatsCommandValidator : AbstractValidator<ReserveSeatsCommand>
    {
        public ReserveSeatsCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
                .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.SeatsIds)
                .NotEmpty().WithMessage("SeatsIds is required.");
        }
    }
}