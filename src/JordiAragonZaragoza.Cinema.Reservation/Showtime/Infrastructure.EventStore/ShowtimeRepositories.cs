namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EventStore
{
    using Autofac;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

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