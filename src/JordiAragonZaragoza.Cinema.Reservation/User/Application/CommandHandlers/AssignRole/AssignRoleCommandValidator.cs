namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AssignRole
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public sealed class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
    {
        public AssignRoleCommandValidator()
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