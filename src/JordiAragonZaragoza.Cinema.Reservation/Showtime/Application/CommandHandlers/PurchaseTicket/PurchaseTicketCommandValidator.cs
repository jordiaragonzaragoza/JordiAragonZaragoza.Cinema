namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.PurchaseTicket
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class PurchaseTicketCommandValidator : BaseValidator<PurchaseTicketCommand>
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