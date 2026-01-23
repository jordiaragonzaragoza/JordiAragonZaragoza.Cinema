namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveUser
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public sealed class RemoveUserCommandValidator : AbstractValidator<RemoveUserCommand>
    {
        public RemoveUserCommandValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");
        }
    }
}