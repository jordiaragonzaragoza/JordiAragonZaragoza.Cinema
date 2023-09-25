namespace JordiAragon.Cinema.Application.Showtime.Queries.GetShowtimes
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Showtime.Queries;

    public class GetShowtimesQueryValidator : AbstractValidator<GetShowtimesQuery>
    {
        public GetShowtimesQueryValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");
        }
    }
}