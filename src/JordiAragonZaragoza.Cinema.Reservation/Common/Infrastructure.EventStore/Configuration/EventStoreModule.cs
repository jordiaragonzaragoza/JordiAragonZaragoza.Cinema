namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Configuration
{
    using System.Reflection;
    using Autofac;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EventStore;
    using JordiAragonZaragoza.SharedKernel;

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