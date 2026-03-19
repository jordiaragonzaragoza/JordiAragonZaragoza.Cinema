namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelShowtime
{
    using System;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using Xunit.Abstractions;

    public sealed class CancelShowtimeScenario
    {
        public Guid ShowtimeId { get; } = Guid.NewGuid();

        public async Task ArrangeAsync(
            ReservationCommandTestClient commandClient,
            ReservationQueryTestClient queryClient,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(commandClient);
            ArgumentNullException.ThrowIfNull(queryClient);

            var sessionDate = DateTimeOffset.UtcNow.AddDays(1);

            var request = new ScheduleShowtimeBodyRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                sessionDate);

            await commandClient.ScheduleShowtimeAsync(this.ShowtimeId, request, output);

            await EventualConsistency.WaitUntilAsync(
                () => queryClient.ShowtimeExistsAsync(this.ShowtimeId, output));
        }
    }
}