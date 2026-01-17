namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;

    public sealed class ShowtimeCommandClient
    {
        private readonly HttpClient http;

        public ShowtimeCommandClient(HttpClient http)
            => this.http = http;

        public async Task ScheduleShowtimeAsync(Guid showtimeId, ScheduleShowtimeBodyRequest request)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.ScheduleShowtime}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var content = StringContentHelpers.FromModelAsJson(request);

            var fullUri = new Uri(this.http.BaseAddress!, route);

            await this.http.PutAsync(fullUri, content);
        }
    }
}