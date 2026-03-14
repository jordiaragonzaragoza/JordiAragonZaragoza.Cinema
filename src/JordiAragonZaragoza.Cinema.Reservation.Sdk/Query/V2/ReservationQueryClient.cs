namespace JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts.HttpClientHelpers;

    public sealed class ReservationQueryClient : IReservationQueryClient
    {
        private readonly HttpClient http;

        public ReservationQueryClient(HttpClient http)
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

        public async Task<PaginatedCollectionResponse<ShowtimeResponse>> GetShowtimesAsync(
            GetShowtimesRequest request,
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(paginatedRequest);

            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimes}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(request.MovieId), request.MovieId?.ToString() ?? string.Empty),
                (nameof(request.AuditoriumId), request.AuditoriumId?.ToString() ?? string.Empty),
                (nameof(request.StartTimeOnUtc), request.StartTimeOnUtc?.ToString("o", CultureInfo.InvariantCulture) ?? string.Empty),
                (nameof(request.EndTimeOnUtc), request.EndTimeOnUtc?.ToString("o", CultureInfo.InvariantCulture) ?? string.Empty),
                (nameof(request.MovieTitle), request.MovieTitle ?? string.Empty),
                (nameof(request.AuditoriumName), request.AuditoriumName ?? string.Empty),
                (nameof(paginatedRequest.PageNumber), paginatedRequest.PageNumber.ToString(CultureInfo.InvariantCulture)),
                (nameof(paginatedRequest.PageSize), paginatedRequest.PageSize.ToString(CultureInfo.InvariantCulture)));

            return await this.http.GetAndDeserializeAsync<PaginatedCollectionResponse<ShowtimeResponse>>(uri.PathAndQuery, cancellationToken);
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

        public async Task<PaginatedCollectionResponse<ReservationResponse>> GetShowtimeReservationsAsync(Guid showtimeId, PaginatedRequest paginatedRequest, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(paginatedRequest);

            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimeReservations}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()),
                (nameof(paginatedRequest.PageNumber), paginatedRequest.PageNumber.ToString(CultureInfo.InvariantCulture)),
                (nameof(paginatedRequest.PageSize), paginatedRequest.PageSize.ToString(CultureInfo.InvariantCulture)));

            return await this.http.GetAndDeserializeAsync<PaginatedCollectionResponse<ReservationResponse>>(uri.PathAndQuery, cancellationToken);
        }
    }
}