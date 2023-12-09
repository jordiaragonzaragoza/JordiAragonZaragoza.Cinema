namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStoreDb.Configuration
{
    public class EventStoreDbOptions
    {
        public const string Section = "EventStore";

        public string ConnectionString { get; set; }
    }
}