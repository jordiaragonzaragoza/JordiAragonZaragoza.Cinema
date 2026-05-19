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
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;
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

        public async Task GrantUserAsync(Guid userId, GrantUserBodyRequest request, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{UserRoutes.GrantUser}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);

            using var content = StringContentHelpers.FromModelAsJson(request);
            var fullUri = new Uri(this.http.BaseAddress!, route);

            using var response = await this.http.PostAsync(fullUri, content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task RevokeUserAsync(Guid userId, RevokeUserBodyRequest request, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{UserRoutes.RevokeUser}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);

            using var content = StringContentHelpers.FromModelAsJson(request);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, new Uri(this.http.BaseAddress!, route))
            {
                Content = content,
            };

            using var response = await this.http.SendAsync(httpRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task AssignRoleAsync(Guid userId, AssignRoleBodyRequest request, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{UserRoutes.AssignRole}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);

            using var content = StringContentHelpers.FromModelAsJson(request);
            var fullUri = new Uri(this.http.BaseAddress!, route);

            using var response = await this.http.PostAsync(fullUri, content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveRoleAsync(Guid userId, RemoveRoleBodyRequest request, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{UserRoutes.RemoveRole}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);

            using var content = StringContentHelpers.FromModelAsJson(request);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, new Uri(this.http.BaseAddress!, route))
            {
                Content = content,
            };

            using var response = await this.http.SendAsync(httpRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task AssignPermissionAsync(Guid userId, AssignPermissionBodyRequest request, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{UserRoutes.AssignPermission}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);

            using var content = StringContentHelpers.FromModelAsJson(request);
            var fullUri = new Uri(this.http.BaseAddress!, route);

            using var response = await this.http.PostAsync(fullUri, content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemovePermissionAsync(Guid userId, RemovePermissionBodyRequest request, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{UserRoutes.RemovePermission}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);

            using var content = StringContentHelpers.FromModelAsJson(request);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, new Uri(this.http.BaseAddress!, route))
            {
                Content = content,
            };

            using var response = await this.http.SendAsync(httpRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
