namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common.Configuration
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

            app.UseSerilogRequestLogging();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            ////app.UseCors(ConfigureCors.CorsPolicy);

            // Must run before MapMcp: besides validating tokens, the MCP scheme answers
            // /.well-known/oauth-protected-resource and adds resource_metadata to the 401 challenge.
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapMcp()
               .RequireAuthorization();

            return app;
        }
    }
}