namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System;
    using System.Reflection;
    using Autofac;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public class EntityFrameworkModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => AssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterBusinessModelRepositories(builder);
            RegisterReadModelsRepositories(builder);
            RegisterDataModelsRepositories(builder);
        }

        private static void RegisterBusinessModelRepositories(ContainerBuilder builder)
        {
            // TODO: Temporal registration. Remove on event sourcing.
            builder.RegisterType<ReservationRepository<Showtime, ShowtimeId>>()
                    .As<IRepository<Showtime, ShowtimeId>>()
                    .InstancePerLifetimeScope();

            // Write Aggregate Repositories
            builder.RegisterType<ReservationRepository<Movie, MovieId>>()
                    .As<IRepository<Movie, MovieId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Auditorium, AuditoriumId>>()
                    .As<IRepository<Auditorium, AuditoriumId>>()
                    .InstancePerLifetimeScope();

            // Read Aggregate Repositories
            builder.RegisterGeneric(typeof(ReservationRepository<,>))
                .As(typeof(IReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationRepository<,>))
                .As(typeof(IReadListRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationRepository<,>))
                .As(typeof(ISpecificationReadRepository<,>))
                .InstancePerLifetimeScope();
        }

        private static void RegisterReadModelsRepositories(ContainerBuilder builder)
        {
            // Write Models Repositories
            builder.RegisterType<ReservationReadModelRepository<ShowtimeReadModel>>()
                    .As<IRepository<ShowtimeReadModel, Guid>>()
                    .InstancePerLifetimeScope();

            // Read Models Repositories
            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IReadListRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(ISpecificationReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IPaginatedSpecificationReadRepository<>))
                .InstancePerLifetimeScope();
        }

        private static void RegisterDataModelsRepositories(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ReservationDataModelCachedSpecificationRepository<>))
                .As(typeof(ICachedSpecificationRepository<,>))
                .InstancePerLifetimeScope();
        }
    }
}