namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.CommandHandlers.AddTenant
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Commands;

    public sealed class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantCommandValidator()
        {
            this.RuleFor(x => x.TenantId)
              .NotEmpty().WithMessage("TenantId is required.");
        }
    }
}