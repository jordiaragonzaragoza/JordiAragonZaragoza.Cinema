namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using ReservationApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses.ReservationResponse;
    using ReserveSeatsBodyApiRequest = JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests.ReserveSeatsBodyRequest;
    using ScheduleShowtimeBodyApiRequest = JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests.ScheduleShowtimeBodyRequest;
    using SeatResponseApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses.SeatResponse;

    public static class ShowtimesCommandMapper
    {
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

        private static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<SeatResponseApiResponse> apiResponse)
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
    }
}