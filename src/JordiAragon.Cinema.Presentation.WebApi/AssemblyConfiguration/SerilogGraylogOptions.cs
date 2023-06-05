namespace JordiAragon.Cinema.Presentation.WebApi.AssemblyConfiguration
{
    using Serilog.Events;

    public class SerilogGraylogOptions
    {
        public const string Section = "Serilog:Graylog";

        public bool Enabled { get; set; }

        public LogEventLevel MinimumLevel { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }
}