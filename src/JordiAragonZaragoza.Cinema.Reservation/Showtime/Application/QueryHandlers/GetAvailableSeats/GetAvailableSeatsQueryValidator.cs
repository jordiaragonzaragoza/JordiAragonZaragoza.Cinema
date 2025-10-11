namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetAvailableSeats
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;

    public sealed class GetAvailableSeatsQueryValidator : AbstractValidator<GetAvailableSeatsQuery>
    {
        public GetAvailableSeatsQueryValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}