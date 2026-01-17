namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.SystemTests.Common.HttpClient;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit.Abstractions;

    public sealed class ShowtimeQueryClient
    {
        private readonly HttpClient http;

        public ShowtimeQueryClient(HttpClient http)
            => this.http = http;

        public async Task<ShowtimeResponse?> GetShowtimeAsync(Guid showtimeId, ITestOutputHelper? output = null)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtime}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            return await this.http.GetAndDeserializeAsync<ShowtimeResponse>(uri.PathAndQuery, output);
        }

        public async Task<bool> ShowtimeExistsAsync(Guid showtimeId, ITestOutputHelper? output = null)
            => await this.GetShowtimeAsync(showtimeId, output) is not null;

        public async Task<bool> ShowtimeNotExistsAsync(Guid showtimeId, ITestOutputHelper? output = null)
            => await this.GetShowtimeAsync(showtimeId, output) is null;

        public async Task<IEnumerable<SeatResponse>?> GetAvailableSeatsAsync(Guid showtimeId, ITestOutputHelper? output = null)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetAvailableSeats}";
            route = route.Replace("{ShowtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            return await this.http.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route, output);
        }

        public async Task<PaginatedCollectionResponse<ReservationResponse>?> GetShowtimeReservationsAsync(Guid showtimeId, ITestOutputHelper? output = null)
        {
            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimeReservations}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            return await this.http.GetAndDeserializeAsync<PaginatedCollectionResponse<ReservationResponse>>(uri.PathAndQuery, output);
        }
    }
}