namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHanders.RemoveUser
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class RemoveUserCommandValidator : BaseValidator<RemoveUserCommand>
    {
        public RemoveUserCommandValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");
        }
    }
}