namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ExpireReservedSeats
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class ExpireReservedSeatsCommandValidator : BaseValidator<ExpireReservedSeatsCommand>
    {
        public ExpireReservedSeatsCommandValidator()
        {
            this.RuleFor(x => x.TicketId)
              .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}