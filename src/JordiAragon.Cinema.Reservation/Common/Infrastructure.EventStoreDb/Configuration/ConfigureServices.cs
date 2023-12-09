namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStoreDb.Configuration
{
    using global::EventStore.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class ConfigureServices
    {
        public static IServiceCollection AddEventStoreServices(this IServiceCollection serviceCollection, IConfiguration configuration, string applicationName)
        {
            var eventStoreDbOptions = new EventStoreDbOptions();
            configuration.Bind(EventStoreDbOptions.Section, eventStoreDbOptions);
            serviceCollection.AddSingleton(Options.Create(eventStoreDbOptions));

            serviceCollection
            ////.AddSingleton(EventTypeMapper.Instance)
            .AddSingleton(new EventStoreClient(EventStoreClientSettings.Create(eventStoreDbOptions.ConnectionString)));
            ////.AddTransient<EventStoreDBSubscriptionToAll, EventStoreDBSubscriptionToAll>();

            return serviceCollection;
        }
    }
}