namespace JordiAragon.Cinema.Application.Features.Showtime.Queries.GetAvailableSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Queries;

    public class GetAvailableSeatsQueryValidator : AbstractValidator<GetAvailableSeatsQuery>
    {
        public GetAvailableSeatsQueryValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}