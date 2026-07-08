namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Configuration
{
    using System;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.Extensions.Configuration;

    public static class ConfigureCors
    {
        public const string CorsPolicy = "CorsPolicy";
        private const string CorsOriginsSettingKey = "CorsOrigins";

        public static CorsPolicy BuildPolicy(IConfiguration configuration)
        {
            var corsOrigins = configuration.GetValue<string>(CorsOriginsSettingKey) ?? "*";

            var builder = new CorsPolicyBuilder() // TODO: Review.
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();

            if (corsOrigins == "*")
            {
                builder.SetIsOriginAllowed(_ => true);
            }
            else
            {
                var origins = corsOrigins
                    .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (origins.Length == 0)
                {
                    throw new InvalidOperationException($"Missing or invalid '{CorsOriginsSettingKey}' in configuration.");
                }

                builder.WithOrigins(origins);
            }

            return builder.Build();
        }
    }
}