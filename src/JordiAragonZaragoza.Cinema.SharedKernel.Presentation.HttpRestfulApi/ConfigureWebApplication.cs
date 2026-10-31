namespace JordiAragonZaragoza.Cinema.SharedKernel.Presentation.HttpRestfulApi
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
        public static WebApplication UseWebApplicationConfigurations(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var apiOptions = app.Services.GetRequiredService<ApiOptions>();

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

            app.UseMiddleware<ExecutionContextMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var group in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse().Select(description => description.GroupName))
                    {
                        options.SwaggerEndpoint($"/swagger/{group}/swagger.json", group.ToUpperInvariant());
                    }

                    options.OAuthClientId(apiOptions.SwaggerClientId);
                    options.OAuthUsePkce();
                    options.OAuthScopes(apiOptions.SwaggerScopes.ToArray());
                });
            }

            app.MapControllers();

            return app;
        }
    }
}
