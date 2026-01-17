namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.Assertions
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SystemTests.Common.ApiClients;

    public static class ShowtimeAssertions
    {
        public static async Task ShouldExistAsync(
            ReservationQueryClient queryClient,
            Guid showtimeId,
            DateTimeOffset expectedSessionDate)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var showtime = await queryClient.GetShowtimeAsync(showtimeId);

            showtime.Should().NotBeNull();
            showtime.SessionDateOnUtc.Should().Be(expectedSessionDate);
            showtime.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
            showtime.AuditoriumId.Should().Be(SeedData.ExampleAuditorium.Id);
            showtime.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);
        }
    }
}