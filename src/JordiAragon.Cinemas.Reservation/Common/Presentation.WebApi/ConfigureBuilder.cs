namespace JordiAragon.Cinemas.Reservation.Common.Presentation.WebApi
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Sinks.Graylog;
    using Serilog.Sinks.Graylog.Core.Transport;

    public static class ConfigureBuilder
    {
        public static IHostBuilder AddWebApiHostBuilderConfigurations(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog(ConfigureSerilog);

            return hostBuilder;
        }

        private static void ConfigureSerilog(
            HostBuilderContext context,
            LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadName()
                .Enrich.WithThreadId()
                .AddConsoleLogger(context.Configuration)
                .AddGraylogLogger(context.Configuration)
                .AddAzureTableStorageLogger(context.Configuration);
        }

        private static LoggerConfiguration AddConsoleLogger(
           this LoggerConfiguration loggerConfiguration,
           IConfiguration configuration)
        {
            var serilogConsoleOptions = new SerilogConsoleOptions();
            configuration.GetSection(SerilogConsoleOptions.Section).Bind(serilogConsoleOptions);

            if (serilogConsoleOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Async(loggerSinkConfiguration =>
                loggerSinkConfiguration.Console(
                    restrictedToMinimumLevel: serilogConsoleOptions.MinimumLevel))
                    .Destructure.ToMaximumStringLength(int.MaxValue);
            }

            return loggerConfiguration;
        }

        private static LoggerConfiguration AddGraylogLogger(
           this LoggerConfiguration loggerConfiguration,
           IConfiguration configuration)
        {
            var serilogGraylogOptions = new SerilogGraylogOptions();
            configuration.GetSection(SerilogGraylogOptions.Section).Bind(serilogGraylogOptions);

            if (serilogGraylogOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Async(loggerSinkConfiguration => loggerSinkConfiguration.Graylog(
                    hostnameOrAddress: serilogGraylogOptions.Host,
                    port: serilogGraylogOptions.Port,
                    transportType: TransportType.Udp,
                    minimumLogEventLevel: serilogGraylogOptions.MinimumLevel)).Destructure.ToMaximumStringLength(int.MaxValue);
            }

            return loggerConfiguration;
        }

        private static LoggerConfiguration AddAzureTableStorageLogger(
            this LoggerConfiguration loggerConfiguration,
            IConfiguration configuration)
        {
            var serilogAzureTableStorageOptions = new SerilogAzureTableStorageOptions();
            configuration.GetSection(SerilogAzureTableStorageOptions.Section).Bind(serilogAzureTableStorageOptions);

            if (serilogAzureTableStorageOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Async(loggerSinkConfiguration => loggerSinkConfiguration.AzureTableStorage(
                    connectionString: serilogAzureTableStorageOptions.BuildConnectionString(),
                    restrictedToMinimumLevel: serilogAzureTableStorageOptions.MinimumLevel,
                    storageTableName: serilogAzureTableStorageOptions.StorageTableName)).Destructure.ToMaximumStringLength(int.MaxValue);
            }

            return loggerConfiguration;
        }
    }
}