namespace JordiAragon.Cinema.Reservation.Features.Auditorium.Infrastructure.EntityFramework
{
    using Autofac;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public static class AuditoriumRepositories
    {
        public static void RegisterBusinessModelRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<ReservationRepository<Auditorium, AuditoriumId>>()
                    .As<IRepository<Auditorium, AuditoriumId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Auditorium, AuditoriumId>>()
                    .As<IReadRepository<Auditorium, AuditoriumId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Auditorium, AuditoriumId>>()
                    .As<IReadListRepository<Auditorium, AuditoriumId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Auditorium, AuditoriumId>>()
                    .As<ISpecificationReadRepository<Auditorium, AuditoriumId>>()
                    .InstancePerLifetimeScope();
        }
    }
}