namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.GrantUser
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public sealed class GrantUserCommandValidator : AbstractValidator<GrantUserCommand>
    {
        public GrantUserCommandValidator()
        {
            this.RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            this.RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("TenantId is required.");

            this.RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("At least one role must be assigned.");
        }
    }
}