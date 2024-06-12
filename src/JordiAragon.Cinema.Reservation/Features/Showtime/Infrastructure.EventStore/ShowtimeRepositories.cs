namespace JordiAragon.Cinema.Reservation.Features.Showtime.Infrastructure.EventStore
{
    using Autofac;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStore;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public static class ShowtimeRepositories
    {
        public static void RegisterBusinessModelRepositories(ContainerBuilder builder)
        {
            // TODO: Temporal disabled.
            /*builder.RegisterType<ReservationRepository<Showtime, ShowtimeId>>()
                    .As<IRepository<Showtime, ShowtimeId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Showtime, ShowtimeId>>()
                    .As<IReadRepository<Showtime, ShowtimeId>>()
                    .InstancePerLifetimeScope();*/
        }
    }
}