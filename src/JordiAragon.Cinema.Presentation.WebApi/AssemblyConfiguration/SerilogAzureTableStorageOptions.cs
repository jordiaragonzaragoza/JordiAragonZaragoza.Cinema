namespace JordiAragon.Cinema.Presentation.WebApi.AssemblyConfiguration
{
    using System.Text;
    using Serilog.Events;

    public class SerilogAzureTableStorageOptions
    {
        public const string Section = "Serilog:AzureTableStorage";

        public bool Enabled { get; set; }

        public LogEventLevel MinimumLevel { get; set; }

        public string StorageTableName { get; set; }

        public string DefaultEndpointsProtocol { get; set; }

        public string AccountName { get; set; }

        public string AccountKey { get; set; }

        public string EndpointSuffix { get; set; }

        public string BuildConnectionString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"DefaultEndpointsProtocol={this.DefaultEndpointsProtocol};");
            stringBuilder.Append($"AccountName={this.AccountName};");
            stringBuilder.Append($"AccountKey={this.AccountKey};");
            stringBuilder.Append($"EndpointSuffix={this.EndpointSuffix};");

            return stringBuilder.ToString();
        }
    }
}