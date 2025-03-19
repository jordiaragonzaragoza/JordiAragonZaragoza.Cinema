namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservation
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class GetUserReservationQueryValidator : BaseValidator<GetUserReservationQuery>
    {
        public GetUserReservationQueryValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");

            this.RuleFor(x => x.ReservationId)
              .NotEmpty().WithMessage("ReservationId is required.");
        }
    }
}