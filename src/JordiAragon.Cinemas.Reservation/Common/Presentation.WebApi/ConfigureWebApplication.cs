namespace JordiAragon.Cinemas.Reservation.Common.Presentation.WebApi
{
    using FastEndpoints;
    using FastEndpoints.Swagger;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using JordiAragon.SharedKernel.Presentation.WebApi.Middlewares;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Hosting;
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