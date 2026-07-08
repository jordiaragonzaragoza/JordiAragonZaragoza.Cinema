namespace JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public interface IReservationQueryClient
    {
        Task<ShowtimeResponse> GetShowtimeAsync(Guid showtimeId, CancellationToken cancellationToken = default);

        Task<PaginatedCollectionResponse<ShowtimeResponse>> GetShowtimesAsync(
            GetShowtimesRequest request,
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken = default);

        Task<ReservationResponse> GetShowtimeReservationAsync(Guid reservationId, CancellationToken cancellationToken = default);

        Task<IEnumerable<SeatResponse>> GetAvailableSeatsAsync(Guid showtimeId, CancellationToken cancellationToken = default);

        Task<PaginatedCollectionResponse<ReservationResponse>> GetShowtimeReservationsAsync(
            Guid showtimeId,
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken = default);

        Task<PaginatedCollectionResponse<ReservationResponse>> GetUserReservationsAsync(
            UserReservationsRequest request,
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken = default);

        Task<ReservationResponse> GetUserReservationAsync(
            UserReservationRequest request,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<UserAuthorizationResponse>> GetUserAuthorizationsAsync(
            UserAuthorizationRequest request,
            CancellationToken cancellationToken = default);

        // TODO: Will be moved. It belongs to the cinema manager bounded context.
        Task<PaginatedCollectionResponse<AuditoriumResponse>> GetAuditoriumsAsync(
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken = default);

        // TODO: Will be moved. It belongs to the catalog bounded context.
        Task<PaginatedCollectionResponse<MovieResponse>> GetMoviesAsync(
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken = default);

        // TODO: Will be moved. It belongs to the management bounded context.
        Task<PaginatedCollectionResponse<UserResponse>> GetUsersAsync(
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken = default);
    }
}