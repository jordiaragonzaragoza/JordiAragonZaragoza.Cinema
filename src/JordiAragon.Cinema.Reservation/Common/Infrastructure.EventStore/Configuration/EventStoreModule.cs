namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStore.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class EventStoreModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => AssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ReservationRepository<Showtime, ShowtimeId>>()
                    .As<IRepository<Showtime, ShowtimeId>>()
                    .InstancePerLifetimeScope();
        }
    }
}