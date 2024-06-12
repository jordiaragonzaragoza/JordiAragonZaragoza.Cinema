namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStore.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.Cinema.Reservation.Features.Showtime.Infrastructure.EventStore;
    using JordiAragon.SharedKernel;

    public sealed class EventStoreModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => AssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            ShowtimeRepositories.RegisterBusinessModelRepositories(builder);
        }
    }
}