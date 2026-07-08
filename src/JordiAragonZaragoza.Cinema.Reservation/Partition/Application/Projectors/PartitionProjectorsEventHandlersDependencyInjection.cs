namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Projectors
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Projectors.Partition;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain.Events;

    public static class PartitionProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddPartitionProjectors(this IServiceCollection services)
        {
            // Partition projection.
            services.AddProjectorEventHandler<PartitionCreatedEvent, PartitionAddedEventProjector>();
            services.AddProjectorEventHandler<PartitionRemovedEvent, PartitionRemovedEventProjector>();

            return services;
        }
    }
}