namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.Query.V2
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.HttpClient;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed class ApiQueryClient
    {
        private readonly HttpClient http;

        public ApiQueryClient(HttpClient http)
        {
            this.http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public async Task<ShowtimeResponse> GetShowtimeAsync(Guid showtimeId, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtime}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            return await this.http.GetAndDeserializeAsync<ShowtimeResponse>(uri.PathAndQuery, cancellationToken);
        }

        public async Task<ReservationResponse> GetShowtimeReservationAsync(Guid reservationId, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimeReservation}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(reservationId), reservationId.ToString()));

            return await this.http.GetAndDeserializeAsync<ReservationResponse>(uri.PathAndQuery, cancellationToken);
        }

        public async Task<IEnumerable<SeatResponse>> GetAvailableSeatsAsync(Guid showtimeId, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetAvailableSeats}";
            route = route.Replace("{ShowtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            return await this.http.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route, cancellationToken);
        }

        public async Task<PaginatedCollectionResponse<ReservationResponse>> GetShowtimeReservationsAsync(Guid showtimeId, CancellationToken cancellationToken = default)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimeReservations}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            return await this.http.GetAndDeserializeAsync<PaginatedCollectionResponse<ReservationResponse>>(uri.PathAndQuery, cancellationToken);
        }
    }
}