namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.Commands.PurchaseSeats
{
    using System;
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands;

    public class PurchaseSeatsCommandValidator : AbstractValidator<PurchaseSeatsCommand>
    {
        public PurchaseSeatsCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
             .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.ShowtimeId)
             .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.TicketId)
             .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}