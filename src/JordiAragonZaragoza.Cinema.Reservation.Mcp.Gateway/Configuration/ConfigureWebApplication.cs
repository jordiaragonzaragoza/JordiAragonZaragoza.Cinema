namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Configuration
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public static class ConfigureWebApplication
    {
        public static WebApplication UseWebApplicationConfigurations(WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            ////app.UseMiddleware<ExceptionMiddleware>();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            ////app.UseCors(ConfigureCors.CorsPolicy);

            ////app.UseAuthentication();

            ////app.UseAuthorization();

            app.MapHealthChecks("/health");

            app.MapMcp();

            return app;
        }
    }
}