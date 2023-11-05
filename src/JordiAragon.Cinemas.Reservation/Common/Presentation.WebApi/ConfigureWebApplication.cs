namespace JordiAragon.Cinemas.Reservation.Common.Presentation.WebApi
{
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using JordiAragon.SharedKernel.Presentation.WebApi.Middlewares;
    using Serilog;
    using FastEndpoints;
    using FastEndpoints.Swagger;
    using Microsoft.AspNetCore.Mvc;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;

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
                ////app.UseDefaultExceptionHandler(); // from FastEndpoints
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionMiddleware>(); // TODO: Review Exception Handler.

            /*
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
            }*/

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors(Constants.AllowAnyOriginPolicy);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseFastEndpoints(config =>
            {
                config.Endpoints.RoutePrefix = Constants.EndpointsRoutePrefix;
                config.Versioning.Prefix = Constants.EndpointsVersioningPrefix;
                config.Versioning.PrependToRoute = true;
                config.Versioning.DefaultVersion = 2;
                config.Errors.UseProblemDetails();

                config.Errors.ResponseBuilder = ResponseBuilderHelper.BuildResponse;
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerGen();
            }

            app.MapHealthChecks("/health");

            return app;
        }
    }
}