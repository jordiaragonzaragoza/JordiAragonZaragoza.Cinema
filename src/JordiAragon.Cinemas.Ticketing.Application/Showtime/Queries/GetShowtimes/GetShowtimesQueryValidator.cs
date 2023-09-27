namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.Queries.GetShowtimes
{
    using FluentValidation;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Queries;

    public class GetShowtimesQueryValidator : AbstractValidator<GetShowtimesQuery>
    {
        public GetShowtimesQueryValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");
        }
    }
}