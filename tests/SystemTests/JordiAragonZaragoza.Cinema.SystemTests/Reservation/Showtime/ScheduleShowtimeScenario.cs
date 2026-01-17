namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime
{
    using System;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.SystemTests.Common.ApiClients;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;

    public sealed class ScheduleShowtimeScenario
    {
        public Guid ShowtimeId { get; } = Guid.NewGuid();

        public DateTimeOffset SessionDate { get; } = DateTimeOffset.UtcNow.AddDays(1);

        public ScheduleShowtimeBodyRequest BuildRequest()
            => new(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                this.SessionDate);

        public async Task ExecuteAsync(ReservationCommandClient commandClient)
        {
            ArgumentNullException.ThrowIfNull(commandClient);

            await commandClient.ScheduleShowtimeAsync(this.ShowtimeId, this.BuildRequest());
        }
    }
}