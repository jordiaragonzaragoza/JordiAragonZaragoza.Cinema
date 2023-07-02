namespace JordiAragon.Cinema.Application.Features.Showtime.Commands.ExpireReservedSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;

    public class ExpireReservedSeatsCommandValidator : AbstractValidator<ExpireReservedSeatsCommand>
    {
        public ExpireReservedSeatsCommandValidator()
        {
            this.RuleFor(x => x.TicketId)
              .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}