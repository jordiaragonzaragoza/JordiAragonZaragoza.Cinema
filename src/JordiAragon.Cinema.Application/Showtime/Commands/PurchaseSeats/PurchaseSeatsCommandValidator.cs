namespace JordiAragon.Cinema.Application.Showtime.Commands.PurchaseSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Showtime.Commands;

    public class PurchaseSeatsCommandValidator : AbstractValidator<PurchaseSeatsCommand>
    {
        public PurchaseSeatsCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
             .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.TicketId)
             .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}