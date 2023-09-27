namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.Commands.CreateShowtime
{
    using FluentValidation;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Commands;

    public class DeleteShowtimeCommandValidator : AbstractValidator<DeleteShowtimeCommand>
    {
        public DeleteShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}