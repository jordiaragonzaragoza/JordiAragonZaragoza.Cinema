namespace JordiAragon.Cinema.Application.Features.Auditorium.Showtime.Queries
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Showtime.Queries;

    public class GetShowtimesQueryValidator : AbstractValidator<GetShowtimesQuery>
    {
        public GetShowtimesQueryValidator()
        {
            this.RuleFor(x => x.AuditoriumId)
              .NotEmpty().WithMessage("AuditoriumId is required.");
        }
    }
}