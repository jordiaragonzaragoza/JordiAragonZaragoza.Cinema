namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.CommandHandlers.RemovePartition;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.CommandHandlers.CreatePartition;

    public static class PartitionCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddPartitionCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<CreatePartitionCommand, CreatePartitionCommandHandler>();
            services.AddCommandHandler<RemovePartitionCommand, RemovePartitionCommandHandler>();

            return services;
        }
    }
}