namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using Autofac;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public static class ShowtimeRepositories
    {
        public static void RegisterBusinessModelRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<ReservationRepository<Showtime, ShowtimeId>>()
                    .As<IRepository<Showtime, ShowtimeId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Showtime, ShowtimeId>>()
                    .As<IReadRepository<Showtime, ShowtimeId>>()
                    .InstancePerLifetimeScope();
        }
    }
}