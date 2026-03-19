namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ReserveSeats.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using Xunit.Abstractions;

    public static class ReservationAssertions
    {
        public static async Task ShouldExistAsync(
            ReservationQueryTestClient queryClient,
            Guid reservationId,
            Guid expectedShowtimeId,
            Guid expectedUserId,
            DateTimeOffset expectedSessionDate,
            IEnumerable<Guid> expectedSeatsIds,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var reservation = await queryClient.GetShowtimeReservationAsync(reservationId, output);

            reservation.Should().NotBeNull();
            reservation.Id.Should().Be(reservationId, "ReservationId should match the created reservation");
            reservation.UserId.Should().Be(expectedUserId, "UserId should match the expected user");
            reservation.ShowtimeId.Should().Be(expectedShowtimeId, "ShowtimeId should match the expected showtime");
            reservation.SessionDateOnUtc.Should().BeCloseTo(expectedSessionDate, TimeSpan.FromMilliseconds(1), "SessionDate should match the expected date");
            reservation.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);
            reservation.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
            reservation.Seats.Select(seatResponse => seatResponse.Id).Should()
                .Contain(expectedSeatsIds, "Reserved seats should be in the reservation");
            reservation.IsPurchased.Should().BeFalse("New reservation should not be purchased");
        }
    }
}