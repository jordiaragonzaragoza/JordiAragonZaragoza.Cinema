namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.AssemblyConfiguration
{
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using JordiAragon.SharedKernel.Presentation.WebApi.Middlewares;
    using Serilog;

    public static class ConfigureWebApplication
    {
        public static WebApplication AddWebApplicationConfigurations(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                app.UseSwaggerUI(options =>
                {
                    foreach (var version in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse().Select(description => description.GroupName))
                    {
                        options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version.ToUpperInvariant());
                    }
                });
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors(Constants.AllowAnyOriginPolicy);

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health");

            return app;
        }
    }
}