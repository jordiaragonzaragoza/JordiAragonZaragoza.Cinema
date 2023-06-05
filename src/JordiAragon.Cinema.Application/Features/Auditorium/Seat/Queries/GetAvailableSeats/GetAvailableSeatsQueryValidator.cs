namespace JordiAragon.Cinema.Application.Features.Auditorium.Seat.Queries.GetAvailableSeats
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Seat.Queries;

    public class GetAvailableSeatsQueryValidator : AbstractValidator<GetAvailableSeatsQuery>
    {
        public GetAvailableSeatsQueryValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");

            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}