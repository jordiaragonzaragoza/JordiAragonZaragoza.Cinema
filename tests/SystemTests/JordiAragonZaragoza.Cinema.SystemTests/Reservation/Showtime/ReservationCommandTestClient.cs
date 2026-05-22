namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2;
    using Xunit.Abstractions;

    public sealed class ReservationCommandTestClient
    {
        private readonly IReservationCommandClient api;

        public ReservationCommandTestClient(IReservationCommandClient api)
            => this.api = api;

        public async Task ScheduleShowtimeAsync(
            Guid showtimeId,
            ScheduleShowtimeBodyRequest request,
            ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Scheduling showtime {showtimeId}");

            await this.api.ScheduleShowtimeAsync(showtimeId, request);
        }

        public async Task CancelShowtimeAsync(Guid showtimeId, ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Deleting showtime {showtimeId}");
            await this.api.CancelShowtimeAsync(showtimeId);
        }

        public async Task<ReservationResponse?> ReserveSeatsAsync(
            Guid reservationId,
            Guid showtimeId,
            ReserveSeatsBodyRequest request,
            ITestOutputHelper? output = null)
        {
            try
            {
                var response = await this.api.ReserveSeatsAsync(reservationId, showtimeId, request);

                output?.WriteLine($"Reservation created: {response.Id}");

                return response;
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

        public async Task PurchaseReservationAsync(Guid showtimeId, Guid reservationId, ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Purchasing reservation {reservationId} for showtime {showtimeId}");
            await this.api.PurchaseReservationAsync(showtimeId, reservationId);
        }

        public async Task GrantUserAsync(Guid userId, GrantUserBodyRequest request, ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Granting user {userId}");
            await this.api.GrantUserAsync(userId, request);
        }

        public async Task RevokeUserAsync(Guid userId, RevokeUserBodyRequest request, ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Revoking user {userId}");
            await this.api.RevokeUserAsync(userId, request);
        }

        public async Task AssignRoleAsync(Guid userId, AssignRoleBodyRequest request, ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Assigning role to user {userId}");
            await this.api.AssignRoleAsync(userId, request);
        }

        public async Task RemoveRoleAsync(Guid userId, RemoveRoleBodyRequest request, ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Removing role from user {userId}");
            await this.api.RemoveRoleAsync(userId, request);
        }

        public async Task AssignPermissionAsync(Guid userId, AssignPermissionBodyRequest request, ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Assigning permission to user {userId}");
            await this.api.AssignPermissionAsync(userId, request);
        }

        public async Task RemovePermissionAsync(Guid userId, RemovePermissionBodyRequest request, ITestOutputHelper? output = null)
        {
            output?.WriteLine($"Removing permission from user {userId}");
            await this.api.RemovePermissionAsync(userId, request);
        }
    }
}