namespace JordiAragon.Cinema.Reservation.Showtime.Application.Queries.GetShowtimes
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;

    public class GetShowtimesQueryValidator : AbstractValidator<GetShowtimesQuery>
    {
        public GetShowtimesQueryValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");
        }
    }
}