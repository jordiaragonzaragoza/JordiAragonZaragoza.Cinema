namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStore.Configuration
{
    public sealed class EventStoreDbOptions
    {
        public const string Section = "EventStore";

        public string ConnectionString { get; set; }
    }
}