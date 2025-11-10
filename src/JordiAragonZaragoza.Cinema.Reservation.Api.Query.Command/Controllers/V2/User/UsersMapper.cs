namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.User
{
    using System;
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public static class UsersMapper
    {
        public static GetUserReservationQuery ToQuery(
            this UserReservationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GetUserReservationQuery(request.UserId, request.ShowtimeId, request.ReservationId);
        }

        public static GetUserReservationsQuery ToQuery(
            this UserReservationsRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GetUserReservationsQuery(
                request.UserId,
                request.ShowtimeId,
                request.StartIntervalTimeOnUtc,
                request.EndIntervalTimeOnUtc,
                request.AuditoriumName,
                request.MovieTitle,
                request.IsPurchased,
                request.PageNumber ?? 1,
                request.PageSize ?? 10);
        }

        public static GetUsersQuery ToQuery(
            this GetUsersRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GetUsersQuery(
                request.PageNumber ?? 1,
                request.PageSize ?? 10);
        }

        public static Result<ReservationResponse> ToResponse(
            this Result<ReservationReadModel> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(userReadModel => new ReservationResponse(
                userReadModel.Id,
                userReadModel.UserId,
                userReadModel.ShowtimeId,
                userReadModel.SessionDateOnUtc,
                userReadModel.AuditoriumName,
                userReadModel.MovieTitle,
                userReadModel.Seats.ToResponse(),
                userReadModel.IsPurchased));
        }

        public static Result<PaginatedCollectionResponse<UserResponse>> ToResponse(
            this Result<PaginatedCollectionOutputDto<UserReadModel>> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(paginatedCollection =>
                new PaginatedCollectionResponse<UserResponse>(
                    paginatedCollection.ActualPage,
                    paginatedCollection.TotalPages,
                    paginatedCollection.TotalItems,
                    paginatedCollection.Items.ToResponse()));
        }

        private static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<SeatReadModel> seats)
        {
            ArgumentNullException.ThrowIfNull(seats);

            return ToResponseIterator(seats);
        }

        private static IEnumerable<UserResponse> ToResponse(
            this IEnumerable<UserReadModel> users)
        {
            ArgumentNullException.ThrowIfNull(users);

            return ToResponseIterator(users);
        }

        private static IEnumerable<SeatResponse> ToResponseIterator(
            IEnumerable<SeatReadModel> seats)
        {
            foreach (var seat in seats)
            {
                yield return new SeatResponse(
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber);
            }
        }

        private static IEnumerable<UserResponse> ToResponseIterator(
            IEnumerable<UserReadModel> users)
        {
            foreach (var user in users)
            {
                yield return new UserResponse(
                    user.Id);
            }
        }
    }
}