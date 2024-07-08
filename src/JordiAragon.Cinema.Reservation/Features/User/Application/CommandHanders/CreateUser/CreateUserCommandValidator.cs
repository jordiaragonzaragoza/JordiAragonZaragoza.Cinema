namespace JordiAragon.Cinema.Reservation.User.Application.CommandHanders.AddUser
{
    using FluentValidation;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Validators;

    public sealed class CreateUserCommandValidator : BaseValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            this.RuleFor(x => x.UserId)
              .NotEmpty().WithMessage("UserId is required.");
        }
    }
}