namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Commands.CreateShowtime
{
    using FluentValidation;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;

    public class DeleteShowtimeCommandValidator : AbstractValidator<DeleteShowtimeCommand>
    {
        public DeleteShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}