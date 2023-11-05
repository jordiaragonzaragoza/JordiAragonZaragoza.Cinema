namespace JordiAragon.Cinemas.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class EntityFrameworkModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => AssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterGeneric(typeof(TicketingRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(TicketingCachedRepository<>))
                .As(typeof(ICachedRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(TicketingReadRepository<>))
                .As(typeof(IReadRepository<>))
                .InstancePerLifetimeScope();
        }
    }
}