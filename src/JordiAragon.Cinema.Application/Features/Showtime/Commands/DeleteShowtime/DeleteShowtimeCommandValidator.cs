namespace JordiAragon.Cinema.Application.Features.Showtime.Commands.CreateShowtime
{
    using FluentValidation;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;

    public class DeleteShowtimeCommandValidator : AbstractValidator<DeleteShowtimeCommand>
    {
        public DeleteShowtimeCommandValidator()
        {
            this.RuleFor(x => x.ShowtimeId)
              .NotEmpty().WithMessage("ShowtimeId is required.");
        }
    }
}