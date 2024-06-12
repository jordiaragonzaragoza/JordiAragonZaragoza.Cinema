namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public static class MigrationsApplier
    {
        public static void Initialize(WebApplication app)
        {
            using var writeScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var readScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var writeContext = writeScope.ServiceProvider.GetRequiredService<ReservationBusinessModelContext>();
            var readContext = readScope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();

            try
            {
                MigrateAndEnsureSqlServerDatabase(writeContext);
                MigrateAndEnsureSqlServerDatabase(readContext);
            }
            catch (Exception exception)
            {
                app.Logger.LogError(exception, "An error occurred applying migrations or creating database. Error: {exceptionMessage}", exception.Message);
            }
        }

        public static void MigrateAndEnsureSqlServerDatabase(DbContext context)
        {
            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }
        }
    }
}

