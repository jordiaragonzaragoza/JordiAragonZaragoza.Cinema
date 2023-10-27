namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Commands.PurchaseTicket
{
    using FluentValidation;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;

    public class PurchaseTicketCommandValidator : AbstractValidator<PurchaseTicketCommand>
    {
        public PurchaseTicketCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
             .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.TicketId)
             .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}