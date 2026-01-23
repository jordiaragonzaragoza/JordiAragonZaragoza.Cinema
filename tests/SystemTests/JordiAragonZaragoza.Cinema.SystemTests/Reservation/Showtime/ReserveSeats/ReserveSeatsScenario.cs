namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ReserveSeats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using Xunit.Abstractions;

    public sealed class ReserveSeatsScenario
    {
        private readonly List<SeatResponse> availableSeatsIds = new();

        public Guid ShowtimeId { get; } = Guid.NewGuid();

        public IEnumerable<SeatResponse> AvailableSeatsIds => this.availableSeatsIds.AsReadOnly();

        // TODO: Temporal. ExpectedUserId setted as constant till have authentication done.
        public Guid ExpectedUserId { get; private set; } = new Guid("08ffddf5-3826-483f-a806-b3144477c7e8");

        public DateTimeOffset SessionDate { get; private set; }

        public async Task ArrangeAsync(
            ShowtimeCommandClient commandClient,
            ShowtimeQueryClient queryClient,
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

            if (availableSeats is not null && availableSeats.Any())
            {
                this.availableSeatsIds.AddRange(availableSeats);

                return;
            }

            output?.WriteLine($"WARNING: No available seats found for showtime {this.ShowtimeId}");
        }
    }
}