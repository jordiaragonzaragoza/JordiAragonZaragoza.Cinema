namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class EntityFrameworkExtensions
    {
        public static IServiceCollection AddEntityFrameworkRepositories(this IServiceCollection services)
        {
            services.AddBusinessModelRepositories()
                    .AddReadModelsRepositories()
                    .AddDataModelsRepositories();

            return services;
        }

        private static IServiceCollection AddBusinessModelRepositories(this IServiceCollection services)
        {
            services.AddMovieBusinessModelRepositories()
                    .AddAuditoriumBusinessModelRepositories()
                    .AddUserBusinessModelRepositories()
                    .AddShowtimeBusinessModelRepositories();

            return services;
        }

        private static IServiceCollection AddReadModelsRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(IReadListRepository<,>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(ISpecificationReadRepository<,>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(IPaginatedSpecificationReadRepository<>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(IRangeableRepository<,>), typeof(ReservationReadModelRepository<>));

            return services;
        }

        private static IServiceCollection AddDataModelsRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(ReservationDataModelRepository<>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(ReservationDataModelRepository<>));
            services.AddScoped(typeof(IReadListRepository<,>), typeof(ReservationDataModelRepository<>));
            services.AddScoped(typeof(ISpecificationReadRepository<,>), typeof(ReservationDataModelRepository<>));
            services.AddScoped(typeof(ICachedSpecificationRepository<,>), typeof(ReservationDataModelCachedSpecificationRepository<>));

            return services;
        }
    }
}