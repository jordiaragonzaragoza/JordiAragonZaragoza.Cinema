namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
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

        public async Task<ReservationResponse?> ReserveSeatsAsync(Guid reservationId, Guid showtimeId, ReserveSeatsBodyRequest reserveSeatsRequest, ITestOutputHelper? output = null)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.ReserveSeats}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            route = route.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            using var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            // Act
            return await this.http.PutAndDeserializeAsync<ReservationResponse>(route, reserveSeatsContent, output);
        }

        public async Task PurchaseReservationAsync(Guid showtimeId, Guid reservationId, ITestOutputHelper? output = null)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.PurchaseReservation}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            route = route.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            var fullUri = new Uri(this.http.BaseAddress!, route);

            output?.WriteLine($"Requesting with PATCH {route}");
            await this.http.PatchAsync(fullUri, null);
        }
    }
}