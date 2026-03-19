namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ScheduleShowtime.Assertions
{
    using System;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using Xunit.Abstractions;

    public static class ShowtimeAssertions
    {
        public static async Task ShouldExistAsync(
            ReservationQueryTestClient queryClient,
            Guid showtimeId,
            DateTimeOffset expectedSessionDate,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var showtime = await queryClient.GetShowtimeAsync(showtimeId, output);

            showtime.Should().NotBeNull();
            showtime.SessionDateOnUtc.Should().BeCloseTo(expectedSessionDate, TimeSpan.FromMilliseconds(1));
            showtime.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
            showtime.AuditoriumId.Should().Be(SeedData.ExampleAuditorium.Id);
            showtime.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);
        }
    }
}