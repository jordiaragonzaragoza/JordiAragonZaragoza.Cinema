namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.User.Tools.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Responses;
    using ApiContracts = JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using McpGatewayContracts = JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1;
    using ReservationApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses.ReservationResponse;
    using SeatResponseApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses.SeatResponse;
    using UserApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Responses.UserResponse;
    using UserReservationApiRequest = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests.UserReservationRequest;
    using UserReservationsApiRequest = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests.UserReservationsRequest;

    public static class UsersQueryMapper
    {
        public static McpGatewayContracts.Common.PaginatedCollectionResponse<UserResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<UserApiResponse> apiResponse)
        {
            return apiResponse.ToPaginatedResponse(x => x.ToResponse());
        }

        public static UserResponse ToResponse(
            this UserApiResponse apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return new UserResponse(
                    apiResponse.Id);
        }

        public static McpGatewayContracts.Common.PaginatedCollectionResponse<ReservationResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<ReservationApiResponse> apiResponse)
        {
            return apiResponse.ToPaginatedResponse(x => x.ToResponse());
        }

        public static ReservationResponse ToResponse(
            this ReservationApiResponse apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return new ReservationResponse(
                    apiResponse.Id,
                    apiResponse.UserId,
                    apiResponse.ShowtimeId,
                    apiResponse.SessionDateOnUtc,
                    apiResponse.AuditoriumName,
                    apiResponse.MovieTitle,
                    apiResponse.Seats.ToResponse(),
                    apiResponse.IsPurchased);
        }

        public static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<SeatResponseApiResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return ToResponseIterator(apiResponse);
        }

        public static UserReservationsApiRequest ToApiRequest(
            this UserReservationsRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new UserReservationsApiRequest(
                    request.UserId,
                    request.ShowtimeId,
                    request.StartIntervalTimeOnUtc,
                    request.EndIntervalTimeOnUtc,
                    request.AuditoriumName,
                    request.MovieTitle,
                    request.IsPurchased);
        }

        public static UserReservationApiRequest ToApiRequest(
            this UserReservationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new UserReservationApiRequest(
                    request.UserId,
                    request.ShowtimeId,
                    request.ReservationId);
        }

        private static IEnumerable<SeatResponse> ToResponseIterator(
            IEnumerable<SeatResponseApiResponse> apiResponse)
        {
            foreach (var seatOutputDto in apiResponse)
            {
                yield return new SeatResponse(
                    seatOutputDto.Id,
                    seatOutputDto.Row,
                    seatOutputDto.SeatNumber);
            }
        }
    }
}