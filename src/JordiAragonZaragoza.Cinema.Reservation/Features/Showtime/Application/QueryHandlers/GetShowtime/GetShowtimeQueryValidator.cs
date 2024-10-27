namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtime
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class GetShowtimeQueryValidator : BaseValidator<GetShowtimeQuery>
    {
        public GetShowtimeQueryValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}