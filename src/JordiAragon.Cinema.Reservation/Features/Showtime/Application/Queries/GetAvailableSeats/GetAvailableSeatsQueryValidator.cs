namespace JordiAragon.Cinema.Reservation.Showtime.Application.Queries.GetAvailableSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;

    public class GetAvailableSeatsQueryValidator : AbstractValidator<GetAvailableSeatsQuery>
    {
        public GetAvailableSeatsQueryValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}