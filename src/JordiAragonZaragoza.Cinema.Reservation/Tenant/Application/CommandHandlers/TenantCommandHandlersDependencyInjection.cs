namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.CommandHandlers.RemoveTenant;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.CommandHandlers.CreateTenant;

    public static class TenantCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddTenantCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<CreateTenantCommand, CreateTenantCommandHandler>();
            services.AddCommandHandler<RemoveTenantCommand, RemoveTenantCommandHandler>();

            return services;
        }
    }
}