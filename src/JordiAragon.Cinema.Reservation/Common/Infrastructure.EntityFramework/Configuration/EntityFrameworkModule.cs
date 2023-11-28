namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class EntityFrameworkModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => AssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterGeneric(typeof(ReservationRepository<,,>))
                .As(typeof(IRepository<,,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationCachedSpecificationRepository<,,>))
                .As(typeof(ICachedSpecificationRepository<,,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationReadRepository<,,>))
                .As(typeof(IReadRepository<,,>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReservationSpecificationReadRepository<,,>))
                .As(typeof(ISpecificationReadRepository<,,>))
                .InstancePerLifetimeScope();
        }
    }
}