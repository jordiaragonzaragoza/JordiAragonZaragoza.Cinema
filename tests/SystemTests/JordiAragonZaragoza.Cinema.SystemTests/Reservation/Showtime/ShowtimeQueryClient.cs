namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit.Abstractions;

    public sealed class ShowtimeQueryClient
    {
        private readonly IReservationQueryClient api;

        public ShowtimeQueryClient(IReservationQueryClient api)
            => this.api = api;

        public async Task<ShowtimeResponse?> GetShowtimeAsync(Guid showtimeId, ITestOutputHelper? output = null)
        {
            try
            {
                output?.WriteLine($"Getting showtime {showtimeId}");
                return await this.api.GetShowtimeAsync(showtimeId);
            }
            catch (Exception ex) when (
                ex is HttpRequestException
                || ex is InvalidOperationException
                || ex is OperationCanceledException)
            {
                output?.WriteLine(ex.ToString());

                return default;
            }
        }

        public async Task<bool> ShowtimeExistsAsync(Guid showtimeId, ITestOutputHelper? output = null)
            => await this.GetShowtimeAsync(showtimeId, output) is not null;

        public async Task<bool> ShowtimeNotExistsAsync(Guid showtimeId, ITestOutputHelper? output = null)
            => await this.GetShowtimeAsync(showtimeId, output) is null;

        public async Task<ReservationResponse?> GetShowtimeReservationAsync(Guid reservationId, ITestOutputHelper? output = null)
        {
            try
            {
                output?.WriteLine($"Getting reservation {reservationId}");
                return await this.api.GetShowtimeReservationAsync(reservationId);
            }
            catch (Exception ex) when (
                ex is HttpRequestException
                || ex is InvalidOperationException
                || ex is OperationCanceledException)
            {
                output?.WriteLine(ex.ToString());

                return default;
            }
        }

        public async Task<bool> ShowtimeReservationExistsAsync(Guid reservationId, ITestOutputHelper? output = null)
            => await this.GetShowtimeReservationAsync(reservationId, output) is not null;

        public async Task<bool> ShowtimeReservationNotExistsAsync(Guid reservationId, ITestOutputHelper? output = null)
            => await this.GetShowtimeReservationAsync(reservationId, output) is null;

        public async Task<bool> PurchasedReservationExistsAsync(Guid reservationId, ITestOutputHelper? output = null)
        {
            var reservation = await this.GetShowtimeReservationAsync(reservationId, output);

            return reservation is not null && reservation.IsPurchased;
        }

        public async Task<IEnumerable<SeatResponse>?> GetAvailableSeatsAsync(Guid showtimeId, ITestOutputHelper? output = null)
        {
            try
            {
                output?.WriteLine($"Getting available seats for showtime {showtimeId}");
                return await this.api.GetAvailableSeatsAsync(showtimeId);
            }
            catch (Exception ex) when (
                ex is HttpRequestException
                || ex is InvalidOperationException
                || ex is OperationCanceledException)
            {
                output?.WriteLine(ex.ToString());

                return default;
            }
        }

        public async Task<PaginatedCollectionResponse<ReservationResponse>?> GetShowtimeReservationsAsync(Guid showtimeId, PaginatedRequest paginatedRequest, ITestOutputHelper? output = null)
        {
            try
            {
                output?.WriteLine($"Getting reservations for showtime {showtimeId}");
                return await this.api.GetShowtimeReservationsAsync(showtimeId, paginatedRequest);
            }
            catch (Exception ex) when (
                ex is HttpRequestException
                || ex is InvalidOperationException
                || ex is OperationCanceledException)
            {
                output?.WriteLine(ex.ToString());

                return default;
            }
        }
    }
}