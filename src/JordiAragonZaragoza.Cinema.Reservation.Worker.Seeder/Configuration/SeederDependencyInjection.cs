namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.Configuration
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore.KurrentDb;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Context;

    public static class SeederDependencyInjection
    {
        public static IServiceCollection AddInfrastructureEventStoreSeeder(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEventStore, KurrentDbEventStore>();
            serviceCollection.AddSingleton<IExecutionContextService, ExecutionContextService>();

            return serviceCollection;
        }
    }
}