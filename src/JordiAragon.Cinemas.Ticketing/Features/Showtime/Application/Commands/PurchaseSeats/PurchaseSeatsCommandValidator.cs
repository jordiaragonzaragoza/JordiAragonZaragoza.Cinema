namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Commands.PurchaseSeats
{
    using FluentValidation;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;

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