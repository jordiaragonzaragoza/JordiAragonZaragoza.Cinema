namespace JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SampleData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<CinemaContext>();
            context.Database.EnsureCreated();

            var exampleMovieId = MovieId.Create(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
            context.Movies.Add(Movie.Create(exampleMovieId, "Inception", "tt1375666", new DateTime(2010, 01, 14), "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe"));

            var auditoriumId1 = AuditoriumId.Create(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53"));
            var exampleAuditorium = Auditorium.Create(auditoriumId1, GenerateSeats(auditoriumId1, 28, 22));

            var exampleShowtime = Showtime.Create(ShowtimeId.Create(new Guid("89b073a7-cfcf-4f2a-b01b-4c7f71a0563b")), exampleMovieId, new DateTime(2023, 1, 1), AuditoriumId.Create(exampleAuditorium.Id.Value));
            context.Showtimes.Add(exampleShowtime);
            ////exampleAuditorium.AddShowtime(exampleShowtime.Id); // TODO: Remove.

            context.Auditoriums.Add(exampleAuditorium);

            var auditoriumId2 = AuditoriumId.Create(Guid.NewGuid());
            context.Auditoriums.Add(Auditorium.Create(auditoriumId2, GenerateSeats(auditoriumId2, 21, 18)));

            var auditoriumId3 = AuditoriumId.Create(Guid.NewGuid());
            context.Auditoriums.Add(Auditorium.Create(auditoriumId3, GenerateSeats(auditoriumId3, 15, 21)));

            context.SaveChanges();
        }

        private static List<Seat> GenerateSeats(AuditoriumId auditoriumId, short rows, short seatsPerRow)
        {
            var seats = new List<Seat>();
            for (short r = 1; r <= rows; r++)
            {
                for (short s = 1; s <= seatsPerRow; s++)
                {
                    seats.Add(Seat.Create(SeatId.Create(Guid.NewGuid()), auditoriumId, r, s));
                }
            }

            return seats;
        }
    }
}
