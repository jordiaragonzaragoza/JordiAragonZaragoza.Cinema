namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EventStore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Infrastructure.EventStore;

    public static class BusinessDependencyInjection
    {
        public static IServiceCollection AddInfrastructureEventStoreRepositories(this IServiceCollection services)
        {
            services.AddShowtimeBusinessRepositories();
            services.AddMovieBusinessRepositories();
            services.AddAuditoriumBusinessRepositories();
            services.AddUserBusinessRepositories();
            services.AddCinemaBusinessRepositories();
            services.AddPartitionBusinessRepositories();
            services.AddTenantBusinessRepositories();

            return services;
        }

        public static IHostApplicationBuilder AddInfrastructureKurrentDbClient(this IHostApplicationBuilder builder)
        {
            builder.AddKurrentDBClient(Constants.ReservationBusinessModelStore);

            return builder;
        }
    }
}