namespace JordiAragonZaragoza.Cinema.SystemTests.Common.ApiClients
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;

    public sealed class ReservationQueryClient
    {
        private readonly HttpClient http;

        public ReservationQueryClient(HttpClient http)
            => this.http = http;

        public async Task<ShowtimeResponse?> GetShowtimeAsync(Guid showtimeId)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtime}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            return await this.http.GetAndDeserializeAsync<ShowtimeResponse>(uri.PathAndQuery);
        }

        public async Task<bool> ShowtimeExistsAsync(Guid showtimeId)
            => await this.GetShowtimeAsync(showtimeId) is not null;

        public async Task<IEnumerable<SeatResponse>?> GetAvailableSeatsAsync(Guid showtimeId)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetAvailableSeats}";
            route = route.Replace("{ShowtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            return await this.http.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route);
        }
    }
}