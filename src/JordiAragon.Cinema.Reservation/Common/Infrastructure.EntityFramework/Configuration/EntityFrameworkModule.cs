namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class EntityFrameworkModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => AssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // TODO: Temporal registration.
            builder.RegisterType<ReservationRepository<Showtime, ShowtimeId>>()
                    .As<IRepository<Showtime, ShowtimeId>>()
                    .InstancePerLifetimeScope();

            // Write Repositories
            builder.RegisterType<ReservationRepository<Movie, MovieId>>()
                    .As<IRepository<Movie, MovieId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Auditorium, AuditoriumId>>()
                    .As<IRepository<Auditorium, AuditoriumId>>()
                    .InstancePerLifetimeScope();

            // Read Repositories
            builder.RegisterGeneric(typeof(ReservationReadRepository<,>))
                .As(typeof(IReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadRepository<,>))
                .As(typeof(IReadListRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadRepository<,>))
                .As(typeof(ISpecificationReadRepository<,>))
                .InstancePerLifetimeScope();

            // TODO: Review. Check which entities are using cache repository.
            builder.RegisterGeneric(typeof(ReservationCachedSpecificationRepository<,>))
                .As(typeof(ICachedSpecificationRepository<,>))
                .InstancePerLifetimeScope();
        }
    }
}