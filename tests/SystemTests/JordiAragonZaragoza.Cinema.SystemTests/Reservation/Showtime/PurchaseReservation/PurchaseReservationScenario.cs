namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.PurchaseReservation
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using Xunit.Abstractions;

    public sealed class PurchaseReservationScenario
    {
        public Guid ShowtimeId { get; } = Guid.NewGuid();

        public Guid ReservationId { get; } = Guid.NewGuid();

        public DateTimeOffset SessionDate { get; private set; }

        public async Task ArrangeAsync(
            ReservationCommandTestClient commandClient,
            ReservationQueryTestClient queryClient,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(commandClient);
            ArgumentNullException.ThrowIfNull(queryClient);

            this.SessionDate = DateTimeOffset.UtcNow.AddDays(1);

            var request = new ScheduleShowtimeBodyRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                this.SessionDate);

            output?.WriteLine($"Scheduling showtime {this.ShowtimeId} for {this.SessionDate:O}");
            await commandClient.ScheduleShowtimeAsync(this.ShowtimeId, request, output);

            await EventualConsistency.WaitUntilAsync(
                () => queryClient.ShowtimeExistsAsync(this.ShowtimeId, output));
            output?.WriteLine($"Showtime {this.ShowtimeId} scheduled successfully");

            var availableSeats = await queryClient.GetAvailableSeatsAsync(this.ShowtimeId, output);
            output?.WriteLine($"Retrieved available seats count: {availableSeats?.Count() ?? 0}");

            if (availableSeats is null || !availableSeats.Any())
            {
                output?.WriteLine($"WARNING: No available seats found for showtime {this.ShowtimeId}");

                return;
            }

            var seatsIds = availableSeats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                         .Take(3).Select(seat => seat.Id).ToList();

            var reserveSeatsRequest = new ReserveSeatsBodyRequest(seatsIds);

            output?.WriteLine($"Reserving seats {string.Join(", ", seatsIds)} for showtime {this.ShowtimeId}");
            await commandClient.ReserveSeatsAsync(this.ReservationId, this.ShowtimeId, reserveSeatsRequest, output);

            await EventualConsistency.WaitUntilAsync(
                () => queryClient.ShowtimeReservationExistsAsync(this.ReservationId, output));
            output?.WriteLine($"Reservation {this.ReservationId} created successfully");
        }
    }
}