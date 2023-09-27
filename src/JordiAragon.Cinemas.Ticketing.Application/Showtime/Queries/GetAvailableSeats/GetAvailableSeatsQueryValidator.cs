namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.Queries.GetAvailableSeats
{
    using FluentValidation;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Queries;

    public class GetAvailableSeatsQueryValidator : AbstractValidator<GetAvailableSeatsQuery>
    {
        public GetAvailableSeatsQueryValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}