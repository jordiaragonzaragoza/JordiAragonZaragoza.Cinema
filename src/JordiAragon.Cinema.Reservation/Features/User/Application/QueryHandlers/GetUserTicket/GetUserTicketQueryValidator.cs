namespace JordiAragon.Cinema.Reservation.User.Application.QueryHandlers.GetUserTicket
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class GetUserTicketQueryValidator : BaseValidator<GetUserTicketQuery>
    {
        public GetUserTicketQueryValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.TicketId)
              .NotEmpty().WithMessage("TicketId is required.");
        }
    }
}