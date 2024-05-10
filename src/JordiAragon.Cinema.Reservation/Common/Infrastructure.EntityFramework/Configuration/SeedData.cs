namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
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

        public static readonly Showtime ExampleShowtime =
            Showtime.Schedule(
                id: ShowtimeId.Create(new Guid("89b073a7-cfcf-4f2a-b01b-4c7f71a0563b")),
                movieId: MovieId.Create(ExampleMovie.Id),
                sessionDateOnUtc: new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, 1, TimeSpan.Zero),
                auditoriumId: AuditoriumId.Create(ExampleAuditorium.Id));

        public static readonly User ExampleUser =
            User.Create(
                id: UserId.Create(new Guid("08ffddf5-3826-483f-a806-b3144477c7e8")));

        public static readonly ShowtimeReadModel ExampleShowtimeReadModel =
            new(
                ExampleShowtime.Id,
                ExampleShowtime.SessionDateOnUtc,
                ExampleMovie.Id,
                ExampleMovie.Title,
                ExampleMovie.Runtime,
                ExampleShowtime.AuditoriumId,
                ExampleAuditorium.Name);

        public static IList<AvailableSeatReadModel> ExampleShowtimeAvailableSeatsReadModel()
        {
            var availableSeats = new List<AvailableSeatReadModel>();
            foreach (var seat in ExampleAuditorium.Seats)
            {
                availableSeats.Add(new AvailableSeatReadModel(
                    Guid.NewGuid(),
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber,
                    ExampleShowtime.Id,
                    ExampleAuditorium.Id,
                    ExampleAuditorium.Name));
            }

            return availableSeats;
        }

        public static void Initialize(WebApplication app, bool isDevelopment)
        {
            using var writeScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var readScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var writeContext = writeScope.ServiceProvider.GetRequiredService<ReservationBusinessModelContext>();
            var readContext = readScope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();

            try
            {
                PopulateBusinessModelTestData(writeContext, isDevelopment);
                PopulateReadModelTestData(readContext, isDevelopment);
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

        public static void PopulateReadModelTestData(ReservationReadModelContext context, bool isDevelopment)
        {
            MigrateAndEnsureSqlServerDatabase(context);

            if (!isDevelopment || HasAnyData(context))
            {
                return;
            }

            SetPreconfiguredReadData(context);
        }

        private static void MigrateAndEnsureSqlServerDatabase(DbContext context)
        {
            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
                context.Database.EnsureCreated();
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
            SetQuartzClusteringSQLServerTables(context);

            context.Movies.Add(ExampleMovie);

            context.Auditoriums.Add(ExampleAuditorium);

            context.Showtimes.Add(ExampleShowtime);

            context.Users.Add(ExampleUser);

            ExampleAuditorium.AddShowtime(ShowtimeId.Create(ExampleShowtime.Id));

            ExampleMovie.AddShowtime(ShowtimeId.Create(ExampleShowtime.Id));

            context.SaveChanges();
        }

        private static void SetPreconfiguredReadData(ReservationReadModelContext context)
        {
            context.Showtimes.Add(ExampleShowtimeReadModel);

            context.AvailableSeats.AddRange(ExampleShowtimeAvailableSeatsReadModel());

            context.SaveChanges();
        }

        private static void SetQuartzClusteringSQLServerTables(DbContext context)
        {
            var tableName = "__QRTZ_LOCKS";
            var tableExists = TableExists(context, tableName);

            if (!tableExists)
            {
                var currentDatabase = context.Database.GetDbConnection().Database;

                var script = GetEmbeddedResource("Common.Infrastructure.EntityFramework.Configuration.QuartzClusteringSQLServerTables.sql");
                script = script.Replace("[currentDatabase]", currentDatabase);

                context.Database.ExecuteSqlRaw(script);
            }
        }

        private static bool TableExists(DbContext context, string tableName)
        {
            var dbConnection = context.Database.GetDbConnection();
            dbConnection.Open();
            var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            var count = (int)dbCommand.ExecuteScalar();
            dbConnection.Close();
            return count > 0;
        }

        private static string GetEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullResourceName = $"{assembly.GetName().Name}.{resourceName}";

            var resourceStream = assembly.GetManifestResourceStream(fullResourceName);

            if (resourceStream == null)
            {
                throw new InvalidOperationException($"Resource '{resourceName}' not found in assembly.");
            }

            using var reader = new StreamReader(resourceStream);
            return reader.ReadToEnd();
        }
    }
}