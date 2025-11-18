namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EventStore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using JordiAragonZaragoza.Cinema.SharedKernel;

    public static class BusinessDependencyInjection
    {
        public static IServiceCollection AddInfrastructureEventStoreDbBusiness(this IServiceCollection services)
        {
            services.AddShowtimeBusinessRepositories();
            services.AddMovieBusinessRepositories();
            services.AddAuditoriumBusinessRepositories();
            services.AddUserBusinessRepositories();

            return services;
        }

        public static IHostApplicationBuilder AddInfrastructureEventStoreDbBusiness(this IHostApplicationBuilder builder)
        {
            builder.AddKurrentDBClient(Constants.JordiAragonZaragozaCinemaReservationBusinessModelStore);

            return builder;
        }
    }
}