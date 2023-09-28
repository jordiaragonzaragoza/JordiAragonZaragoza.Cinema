namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Queries.GetShowtimes
{
    using FluentValidation;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Queries;

    public class GetShowtimesQueryValidator : AbstractValidator<GetShowtimesQuery>
    {
        public GetShowtimesQueryValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");
        }
    }
}