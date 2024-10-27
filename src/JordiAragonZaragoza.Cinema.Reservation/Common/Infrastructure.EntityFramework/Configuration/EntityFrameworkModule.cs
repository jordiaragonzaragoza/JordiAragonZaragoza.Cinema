namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.DataModel;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.SharedKernel;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

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