namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.QueryHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.QueryHandlers.GetCinemas;

    public static class CinemaQueryHandlersDependencyInjection
    {
        public static IServiceCollection AddCinemaQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler<GetCinemasQuery, PaginatedCollectionOutputDto<CinemaReadModel>, GetCinemasQueryHandler>();

            return services;
        }
    }
}