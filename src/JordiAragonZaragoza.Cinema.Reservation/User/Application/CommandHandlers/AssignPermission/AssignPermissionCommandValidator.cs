namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AssignPermission
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public sealed class AssignPermissionCommandValidator : AbstractValidator<AssignPermissionCommand>
    {
        public AssignPermissionCommandValidator()
        {
            this.RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            this.RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("TenantId is required.");

            this.RuleFor(x => x.Permission)
                .NotEmpty().WithMessage("Permission is required.");
        }
    }
}
