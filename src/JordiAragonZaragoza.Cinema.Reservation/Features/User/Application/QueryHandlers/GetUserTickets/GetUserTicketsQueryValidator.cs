namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserTickets
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class GetUserTicketsQueryValidator : BasePaginatedQueryValidator<GetUserTicketsQuery>
    {
        public GetUserTicketsQueryValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");
        }
    }
}