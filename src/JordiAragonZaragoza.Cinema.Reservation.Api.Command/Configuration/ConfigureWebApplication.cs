namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Configuration
{
    using System;
    using System.Linq;
    using Asp.Versioning.ApiExplorer;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Middlewares;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
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

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors(ConfigureCors.CorsPolicy);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<UserContextMiddleware>();
            app.UseMiddleware<PartitionContextMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var version in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse().Select(description => description.GroupName))
                    {
                        options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version.ToUpperInvariant());
                    }
                });
            }

            app.MapHealthChecks("/health");
            app.MapControllers();

            return app;
        }
    }
}