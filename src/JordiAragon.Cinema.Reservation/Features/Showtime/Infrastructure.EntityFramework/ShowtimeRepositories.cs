namespace JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using Autofac;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Contracts.Repositories;

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