namespace JordiAragon.Cinema.Reservation.Common.Presentation.HttpRestfulApi
{
    using Microsoft.AspNetCore.Hosting;

    public static class ConfigureWebHostBuilder
    {
        public static IWebHostBuilder AddWebHostBuilderConfigurations(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.UseKestrel(options => options.AddServerHeader = false);

            return hostBuilder;
        }
    }
}
