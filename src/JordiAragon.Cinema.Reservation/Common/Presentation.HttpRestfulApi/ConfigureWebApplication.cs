namespace JordiAragon.Cinema.Reservation.Common.Presentation.HttpRestfulApi
{
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using FastEndpoints.Swagger;
    using JordiAragon.Cinema.ServiceDefaults;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Middlewares;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public static class ConfigureWebApplication
    {
        public static WebApplication AddWebApplicationConfigurations(WebApplication app)
        {
            Guard.Against.Null(app, nameof(app));

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

            ////app.MapHealthChecks("/health");
            app.MapDefaultEndpoints();

            return app;
        }
    }
}