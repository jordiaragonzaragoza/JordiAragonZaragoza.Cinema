namespace JordiAragon.Cinema.Reservation.User.Application.QueryHandlers.GetUserTickets
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class GetUserTicketsQueryValidator : BasePaginatedQueryValidator<GetUserTicketsQuery>
    {
        public GetUserTicketsQueryValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");
        }
    }
}