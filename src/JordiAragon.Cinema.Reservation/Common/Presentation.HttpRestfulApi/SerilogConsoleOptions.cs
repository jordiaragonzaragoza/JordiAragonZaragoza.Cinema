namespace JordiAragon.Cinema.Reservation.Common.Presentation.HttpRestfulApi
{
    using Serilog.Events;

    public sealed class SerilogConsoleOptions
    {
        public const string Section = "Serilog:Console";

        public bool Enabled { get; set; }

        public LogEventLevel MinimumLevel { get; set; }
    }
}