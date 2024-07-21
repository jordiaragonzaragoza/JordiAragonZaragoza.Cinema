namespace JordiAragon.Cinema.Reservation.Common.Presentation.HttpRestfulApi
{
    using Serilog.Events;

    public sealed class SerilogGraylogOptions
    {
        public const string Section = "Serilog:Graylog";

        public bool Enabled { get; set; }

        public LogEventLevel MinimumLevel { get; set; }

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }
    }
}