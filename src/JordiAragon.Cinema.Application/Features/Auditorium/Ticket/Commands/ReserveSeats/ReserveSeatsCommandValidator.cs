namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.Commands.ReserveSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands;

    public class ReserveSeatsCommandValidator : AbstractValidator<ReserveSeatsCommand>
    {
        public ReserveSeatsCommandValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
                .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.ShowtimeId)
                .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.SeatsIds)
                .NotEmpty().WithMessage("SeatsIds is required.");
        }
    }
}