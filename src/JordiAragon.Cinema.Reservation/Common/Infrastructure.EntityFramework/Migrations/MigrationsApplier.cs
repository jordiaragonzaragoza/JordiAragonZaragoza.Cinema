namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public static class MigrationsApplier
    {
        public static void Initialize(WebApplication app, bool isDevelopment)
        {
            if (!isDevelopment)
            {
                return;
            }

            using var writeScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var readScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            using var writeContext = writeScope.ServiceProvider.GetRequiredService<ReservationBusinessModelContext>();
            using var readContext = readScope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();

            try
            {
                ApplyMigrations(writeContext);
                ApplyMigrations(readContext);
            }
            catch (Exception exception)
            {
                app.Logger.LogError(exception, "An error occurred applying migrations or creating database. Error: {ExceptionMessage}", exception.Message);
            }
        }

        private static void ApplyMigrations(DbContext context)
        {
            context.Database.Migrate();
        }
    }
}

