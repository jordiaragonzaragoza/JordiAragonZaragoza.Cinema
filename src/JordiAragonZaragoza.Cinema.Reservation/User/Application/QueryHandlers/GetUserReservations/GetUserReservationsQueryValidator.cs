namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservations
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class GetUserReservationsQueryValidator : BasePaginatedQueryValidator<GetUserReservationsQuery>
    {
        public GetUserReservationsQueryValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");
        }
    }
}