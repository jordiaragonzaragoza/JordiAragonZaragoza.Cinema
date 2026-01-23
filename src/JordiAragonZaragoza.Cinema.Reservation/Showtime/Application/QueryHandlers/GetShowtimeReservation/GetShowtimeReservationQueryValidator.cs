namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeReservation
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;

    public sealed class GetShowtimeReservationQueryValidator : AbstractValidator<GetShowtimeReservationQuery>
    {
        public GetShowtimeReservationQueryValidator()
        {
            this.RuleFor(x => x.ReservationId)
              .NotEmpty().WithMessage("ReservationId is required.");
        }
    }
}