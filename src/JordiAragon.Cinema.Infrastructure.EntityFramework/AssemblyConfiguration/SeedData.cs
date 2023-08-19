namespace JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class SeedData
    {
        public static readonly Movie ExampleMovie =
            Movie.Create(
                id: MovieId.Create(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")),
                title: "Inception",
                imdbId: "tt1375666",
                releaseDateOnUtc: new DateTime(2010, 01, 14, 1, 1, 1, 1, DateTimeKind.Utc),
                stars: "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe");

        public static readonly Auditorium ExampleAuditorium =
            Auditorium.Create(
                id: AuditoriumId.Create(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53")),
                rows: 28,
                seatsPerRow: 22);

        public static readonly Showtime ExampleShowtime =
            Showtime.Create(
                id: ShowtimeId.Create(new Guid("89b073a7-cfcf-4f2a-b01b-4c7f71a0563b")),
                movieId: MovieId.Create(ExampleMovie.Id.Value),
                sessionDateOnUtc: new DateTime(2023, 1, 1, 1, 1, 1, 1, DateTimeKind.Utc),
                auditoriumId: AuditoriumId.Create(ExampleAuditorium.Id.Value));

        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<CinemaContext>();
            PopulateTestData(context);
        }

        public static void PopulateTestData(CinemaContext context)
        {
            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
                context.Database.EnsureCreated();
            }

            if (HasAnyData(context))
            {
                return;
            }

            SetPreconfiguredData(context);
        }

        private static bool HasAnyData(CinemaContext context)
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

        private static void SetPreconfiguredData(CinemaContext context)
        {
            context.Movies.Add(ExampleMovie);

            context.Auditoriums.Add(ExampleAuditorium);

            context.Showtimes.Add(ExampleShowtime);

            ExampleAuditorium.AddShowtime(ShowtimeId.Create(ExampleShowtime.Id.Value));

            ExampleMovie.AddShowtime(ShowtimeId.Create(ExampleShowtime.Id.Value));

            context.SaveChanges();
        }
    }
}