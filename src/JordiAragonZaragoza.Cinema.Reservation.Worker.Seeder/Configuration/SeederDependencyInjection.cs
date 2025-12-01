namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.Configuration
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore.EventStoreDb;

    public static class SeederDependencyInjection
    {
        public static IServiceCollection AddInfrastructureEventStoreSeeder(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEventStore, EventStoreDbEventStore>();

            return serviceCollection;
        }
    }
}