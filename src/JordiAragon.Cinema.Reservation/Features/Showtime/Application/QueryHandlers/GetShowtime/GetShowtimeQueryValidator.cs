namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtime
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class GetShowtimeQueryValidator : BaseValidator<GetShowtimeQuery>
    {
        public GetShowtimeQueryValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}