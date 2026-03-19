namespace JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts.HttpClientHelpers;

    public sealed class ReservationCommandClient : IReservationCommandClient
    {
        private readonly HttpClient http;

        public ReservationCommandClient(HttpClient http)
        {
            this.http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task ScheduleShowtimeAsync(Guid showtimeId, ScheduleShowtimeBodyRequest request, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.ScheduleShowtime}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            using var content = StringContentHelpers.FromModelAsJson(request);

            var fullUri = new Uri(this.http.BaseAddress!, route);

            using var response = await this.http.PutAsync(fullUri, content, cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        public async Task CancelShowtimeAsync(Guid showtimeId, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.CancelShowtime}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var fullUri = new Uri(this.http.BaseAddress!, route);

            using var response = await this.http.DeleteAsync(fullUri, cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        public async Task<ReservationResponse> ReserveSeatsAsync(Guid reservationId, Guid showtimeId, ReserveSeatsBodyRequest reserveSeatsRequest, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.ReserveSeats}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            route = route.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            using var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            return await this.http.PutAndDeserializeAsync<ReservationResponse>(route, reserveSeatsContent, cancellationToken);
        }

        public async Task PurchaseReservationAsync(Guid showtimeId, Guid reservationId, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.PurchaseReservation}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            route = route.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            var fullUri = new Uri(this.http.BaseAddress!, route);

            using var response = await this.http.PatchAsync(fullUri, null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}