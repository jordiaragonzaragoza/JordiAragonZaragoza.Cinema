namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using ApiContracts = JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using GetShowtimesApiRequest = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests.GetShowtimesRequest;
    using McpGatewayContracts = JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1;
    using ReservationApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses.ReservationResponse;
    using SeatResponseApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses.SeatResponse;
    using ShowtimeApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses.ShowtimeResponse;

    public static class ShowtimesQueryMapper
    {
        public static GetShowtimesApiRequest ToApiRequest(this GetShowtimesRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GetShowtimesApiRequest(
                request.AuditoriumId,
                request.MovieId,
                request.StartTimeOnUtc,
                request.EndTimeOnUtc,
                request.MovieTitle,
                request.AuditoriumName);
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

        public static ShowtimeResponse ToResponse(
            this ShowtimeApiResponse apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return new ShowtimeResponse(
                    apiResponse.Id,
                    apiResponse.MovieTitle,
                    apiResponse.SessionDateOnUtc,
                    apiResponse.AuditoriumId,
                    apiResponse.AuditoriumName);
        }

        public static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<SeatResponseApiResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return ToResponseIterator(apiResponse);
        }

        public static McpGatewayContracts.Common.PaginatedCollectionResponse<ReservationResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<ReservationApiResponse> apiResponse)
        {
            return apiResponse.ToPaginatedResponse(x => x.ToResponse());
        }

        public static McpGatewayContracts.Common.PaginatedCollectionResponse<ShowtimeResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<ShowtimeApiResponse> apiResponse)
        {
            return apiResponse.ToPaginatedResponse(x => x.ToResponse());
        }

        public static IEnumerable<ReservationResponse> ToResponse(
            this IEnumerable<ReservationApiResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return ToResponseIterator(apiResponse);
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

        private static IEnumerable<ReservationResponse> ToResponseIterator(
            IEnumerable<ReservationApiResponse> apiResponse)
        {
            foreach (var reservation in apiResponse)
            {
                yield return reservation.ToResponse();
            }
        }
    }
}