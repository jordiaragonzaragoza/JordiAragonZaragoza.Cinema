namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetAvailableSeats
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class GetAvailableSeatsQueryValidator : BaseValidator<GetAvailableSeatsQuery>
    {
        public GetAvailableSeatsQueryValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}