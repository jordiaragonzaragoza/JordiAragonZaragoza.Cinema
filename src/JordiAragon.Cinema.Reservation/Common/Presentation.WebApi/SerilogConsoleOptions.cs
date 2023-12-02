namespace JordiAragon.Cinema.Reservation.Common.Presentation.WebApi
{
    using Serilog.Events;

    public class SerilogConsoleOptions
    {
        public const string Section = "Serilog:Console";

        public bool Enabled { get; set; }

        public LogEventLevel MinimumLevel { get; set; }
    }
}