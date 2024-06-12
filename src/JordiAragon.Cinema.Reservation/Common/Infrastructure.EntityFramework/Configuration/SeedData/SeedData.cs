namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration.SeedData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class SeedData
    {
        public static readonly Movie ExampleMovie =
            Movie.Create(
                id: MovieId.Create(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")),
                title: "Inception",
                runtime: TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28),
                exhibitionPeriod: ExhibitionPeriod.Create(
                    StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, TimeSpan.Zero)),
                    EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2)),
                    TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28)));

        public static readonly Auditorium ExampleAuditorium =
            Auditorium.Create(
                id: AuditoriumId.Create(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53")),
                name: "Auditorium One",
                rows: 10,
                seatsPerRow: 10);

        public static readonly User ExampleUser =
            User.Create(
                id: UserId.Create(new Guid("08ffddf5-3826-483f-a806-b3144477c7e8")));

        public static void Initialize(WebApplication app, bool isDevelopment)
        {
            using var writeScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var readScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var writeContext = writeScope.ServiceProvider.GetRequiredService<ReservationBusinessModelContext>();
            var readContext = readScope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();

            try
            {
                PopulateBusinessModelTestData(writeContext, isDevelopment);
                PopulateReadModelTestData(readContext);
            }
            catch (Exception exception)
            {
                app.Logger.LogError(exception, "An error occurred seeding the database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }

        public static void PopulateBusinessModelTestData(ReservationBusinessModelContext context, bool isDevelopment)
        {
            MigrateAndEnsureSqlServerDatabase(context);

            if (!isDevelopment || HasAnyData(context))
            {
                return;
            }

            SetPreconfiguredWriteData(context);
        }

        public static void PopulateReadModelTestData(ReservationReadModelContext context)
        {
            MigrateAndEnsureSqlServerDatabase(context);
        }

        private static void MigrateAndEnsureSqlServerDatabase(DbContext context)
        {
            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }
        }

        private static bool HasAnyData(DbContext context)
        {
            var dbSets = context.GetType().GetProperties()
                                           .Where(p => p.PropertyType.IsGenericType
                                                    && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProperty in dbSets)
            {
                var dbSet = (IEnumerable<object>)dbSetProperty.GetValue(context);

                if (dbSet.Any())
                {
                    return true;
                }
            }

            return false;
        }

        private static void SetPreconfiguredWriteData(ReservationBusinessModelContext context)
        {
            context.Movies.Add(ExampleMovie);

            context.Auditoriums.Add(ExampleAuditorium);

            context.Users.Add(ExampleUser);

            context.SaveChanges();
        }
    }
}