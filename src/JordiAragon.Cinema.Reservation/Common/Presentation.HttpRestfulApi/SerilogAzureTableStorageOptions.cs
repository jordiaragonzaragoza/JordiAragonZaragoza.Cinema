namespace JordiAragon.Cinema.Reservation.Common.Presentation.HttpRestfulApi
{
    using System.Text;
    using Serilog.Events;

    public sealed class SerilogAzureTableStorageOptions
    {
        public const string Section = "Serilog:AzureTableStorage";

        public bool Enabled { get; set; }

        public LogEventLevel MinimumLevel { get; set; }

        public string StorageTableName { get; set; } = default!;

        public string DefaultEndpointsProtocol { get; set; } = default!;

        public string AccountName { get; set; } = default!;

        public string AccountKey { get; set; } = default!;

        public string EndpointSuffix { get; set; } = default!;

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