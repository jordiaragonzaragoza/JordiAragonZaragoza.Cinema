namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AddUser
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");
        }
    }
}