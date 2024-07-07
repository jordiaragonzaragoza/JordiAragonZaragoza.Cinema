namespace JordiAragon.Cinema.Reservation.User.Application.CommandHanders.RemoveUser
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class RemoveUserCommandValidator : BaseValidator<RemoveUserCommand>
    {
        public RemoveUserCommandValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");
        }
    }
}