namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveRole
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public sealed class RemoveRoleCommandValidator : AbstractValidator<RemoveRoleCommand>
    {
        public RemoveRoleCommandValidator()
        {
            this.RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            this.RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("TenantId is required.");

            this.RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.");
        }
    }
}