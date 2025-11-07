namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.QueryHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.QueryHandlers.GetAuditoriums;

    public static class AuditoriumQueryHandlersDependencyInjection
    {
        public static IServiceCollection AddAuditoriumQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler<GetAuditoriumsQuery, PaginatedCollectionOutputDto<AuditoriumReadModel>, GetAuditoriumsQueryHandler>();

            return services;
        }
    }
}