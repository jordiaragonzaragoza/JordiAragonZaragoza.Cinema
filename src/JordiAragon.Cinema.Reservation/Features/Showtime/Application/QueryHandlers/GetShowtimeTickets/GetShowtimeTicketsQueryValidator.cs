namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeTickets
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class GetShowtimeTicketsQueryValidator : BasePaginatedQueryValidator<GetShowtimeTicketsQuery>
    {
        public GetShowtimeTicketsQueryValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}