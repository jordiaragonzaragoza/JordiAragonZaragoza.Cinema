namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using ApiContracts = JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using GetShowtimesApiRequest = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests.GetShowtimesRequest;
    using McpGatewayContracts = JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1;
    using ReservationApiCommandResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses.ReservationResponse;
    using ReservationApiQueryResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses.ReservationResponse;
    using ReserveSeatsBodyApiRequest = JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests.ReserveSeatsBodyRequest;
    using ScheduleShowtimeBodyApiRequest = JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests.ScheduleShowtimeBodyRequest;
    using SeatResponseApiCommandResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses.SeatResponse;
    using SeatResponseApiQueryResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses.SeatResponse;
    using ShowtimeApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses.ShowtimeResponse;

    public static class ShowtimesMapper
    {
        public static ApiContracts.PaginatedRequest ToApiRequest(this McpGatewayContracts.Common.PaginatedRequest paginatedRequest)
        {
            ArgumentNullException.ThrowIfNull(paginatedRequest);

            return new ApiContracts.PaginatedRequest()
            {
                PageNumber = paginatedRequest.PageNumber,
                PageSize = paginatedRequest.PageSize,
            };
        }

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

        public static ScheduleShowtimeBodyApiRequest ToApiRequest(this ScheduleShowtimeBodyRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new ScheduleShowtimeBodyApiRequest(
                request.AuditoriumId,
                request.MovieId,
                request.SessionDateOnUtc);
        }

        public static ReserveSeatsBodyApiRequest ToApiRequest(this ReserveSeatsBodyRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new ReserveSeatsBodyApiRequest(
                request.SeatsIds);
        }

        public static ReservationResponse ToResponse(
            this ReservationApiCommandResponse apiResponse)
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

        public static ReservationResponse ToResponse(
            this ReservationApiQueryResponse apiResponse)
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
            this IEnumerable<SeatResponseApiQueryResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return ToResponseIterator(apiResponse);
        }

        public static McpGatewayContracts.Common.PaginatedCollectionResponse<ReservationResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<ReservationApiQueryResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return new McpGatewayContracts.Common.PaginatedCollectionResponse<ReservationResponse>(
                    apiResponse.ActualPage,
                    apiResponse.TotalPages,
                    apiResponse.TotalItems,
                    apiResponse.Items.ToResponse());
        }

        public static McpGatewayContracts.Common.PaginatedCollectionResponse<ShowtimeResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<ShowtimeApiResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return new McpGatewayContracts.Common.PaginatedCollectionResponse<ShowtimeResponse>(
                    apiResponse.ActualPage,
                    apiResponse.TotalPages,
                    apiResponse.TotalItems,
                    apiResponse.Items.ToResponse());
        }

        public static IEnumerable<ReservationResponse> ToResponse(
            this IEnumerable<ReservationApiQueryResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return ToResponseIterator(apiResponse);
        }

        private static IEnumerable<ShowtimeResponse> ToResponse(
            this IEnumerable<ShowtimeApiResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return ToResponseIterator(apiResponse);
        }

        private static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<SeatResponseApiCommandResponse> apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return ToResponseIterator(apiResponse);
        }

        private static IEnumerable<SeatResponse> ToResponseIterator(
            IEnumerable<SeatResponseApiCommandResponse> apiResponse)
        {
            foreach (var seatOutputDto in apiResponse)
            {
                yield return new SeatResponse(
                    seatOutputDto.Id,
                    seatOutputDto.Row,
                    seatOutputDto.SeatNumber);
            }
        }

        private static IEnumerable<SeatResponse> ToResponseIterator(
            IEnumerable<SeatResponseApiQueryResponse> apiResponse)
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
            IEnumerable<ReservationApiQueryResponse> apiResponse)
        {
            foreach (var reservation in apiResponse)
            {
                yield return reservation.ToResponse();
            }
        }

        private static IEnumerable<ShowtimeResponse> ToResponseIterator(
            IEnumerable<ShowtimeApiResponse> apiResponse)
        {
            foreach (var showtime in apiResponse)
            {
                yield return showtime.ToResponse();
            }
        }
    }
}