namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragon.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.Movie.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.User.Infrastructure.EntityFramework;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class EntityFrameworkModule : AssemblyModule
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
            MovieRepositories.RegisterBusinessModelRepositories(builder);
            AuditoriumRepositories.RegisterBusinessModelRepositories(builder);
            UserRepositories.RegisterBusinessModelRepositories(builder);
            ShowtimeRepositories.RegisterBusinessModelRepositories(builder);
        }

        private static void RegisterReadModelsRepositories(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();

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

            builder.RegisterGeneric(typeof(ReservationReadModelRepository<>))
                .As(typeof(IRangeableRepository<,>))
                .InstancePerLifetimeScope();
        }

        private static void RegisterDataModelsRepositories(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ReservationDataModelRepository<>))
                .As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationDataModelRepository<>))
                .As(typeof(IReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationDataModelRepository<>))
                .As(typeof(IReadListRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationDataModelRepository<>))
                .As(typeof(ISpecificationReadRepository<,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationDataModelCachedSpecificationRepository<>))
                .As(typeof(ICachedSpecificationRepository<,>))
                .InstancePerLifetimeScope();
        }
    }
}