namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.SystemTests.Common.HttpClient;
    using Xunit.Abstractions;

    public sealed class ShowtimeCommandClient
    {
        private readonly HttpClient http;

        public ShowtimeCommandClient(HttpClient http)
            => this.http = http;

        public async Task ScheduleShowtimeAsync(Guid showtimeId, ScheduleShowtimeBodyRequest request, ITestOutputHelper? output = null)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.ScheduleShowtime}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            using var content = StringContentHelpers.FromModelAsJson(request);

            var fullUri = new Uri(this.http.BaseAddress!, route);

            output?.WriteLine($"Requesting with PUT {route}");
            await this.http.PutAsync(fullUri, content);
        }

        public async Task CancelShowtimeAsync(Guid showtimeId, ITestOutputHelper? output = null)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.CancelShowtime}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var fullUri = new Uri(this.http.BaseAddress!, route);

            output?.WriteLine($"Requesting with DELETE {route}");
            await this.http.DeleteAsync(fullUri);
        }
    }
}