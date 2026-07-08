namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.CommandHandlers.RemoveTenant
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Commands;

    public sealed class RemoveTenantCommandValidator : AbstractValidator<RemoveTenantCommand>
    {
        public RemoveTenantCommandValidator()
        {
            this.RuleFor(x => x.TenantId)
              .NotEmpty().WithMessage("TenantId is required.");
        }
    }
}