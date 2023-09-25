namespace JordiAragon.Cinema.Application.Showtime.Commands.CreateShowtime
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Showtime.Commands;

    public class DeleteShowtimeCommandValidator : AbstractValidator<DeleteShowtimeCommand>
    {
        public DeleteShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}