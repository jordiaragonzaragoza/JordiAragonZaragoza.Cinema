namespace JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;

    public interface IReservationCommandClient
    {
        Task ScheduleShowtimeAsync(Guid showtimeId, ScheduleShowtimeBodyRequest request, CancellationToken cancellationToken = default);

        Task CancelShowtimeAsync(Guid showtimeId, CancellationToken cancellationToken = default);

        Task<ReservationResponse> ReserveSeatsAsync(Guid reservationId, Guid showtimeId, ReserveSeatsBodyRequest reserveSeatsRequest, CancellationToken cancellationToken = default);

        Task PurchaseReservationAsync(Guid showtimeId, Guid reservationId, CancellationToken cancellationToken = default);

        Task GrantUserAsync(Guid userId, GrantUserBodyRequest request, CancellationToken cancellationToken = default);

        Task RevokeUserAsync(Guid userId, RevokeUserBodyRequest request, CancellationToken cancellationToken = default);

        Task AssignRoleAsync(Guid userId, AssignRoleBodyRequest request, CancellationToken cancellationToken = default);

        Task RemoveRoleAsync(Guid userId, RemoveRoleBodyRequest request, CancellationToken cancellationToken = default);

        Task AssignPermissionAsync(Guid userId, AssignPermissionBodyRequest request, CancellationToken cancellationToken = default);

        Task RemovePermissionAsync(Guid userId, RemovePermissionBodyRequest request, CancellationToken cancellationToken = default);
    }
}