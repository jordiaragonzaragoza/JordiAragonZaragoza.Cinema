namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;

    public static class ShowtimesMapper
    {
        public static ReserveSeatsCommand ToCommand(this ReserveSeatsBodyRequest request, Guid showtimeId, Guid reservationId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new ReserveSeatsCommand(
                reservationId,
                showtimeId,
                request.SeatsIds);
        }

        public static ScheduleShowtimeCommand ToCommand(this ScheduleShowtimeBodyRequest request, Guid showtimeId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new ScheduleShowtimeCommand(
                showtimeId,
                request.AuditoriumId,
                request.MovieId,
                request.SessionDateOnUtc);
        }

        public static Result<ReservationResponse> ToResponse(
            this Result<ReservationOutputDto> resultOutputDto)
        {
            ArgumentNullException.ThrowIfNull(resultOutputDto);

            return resultOutputDto.Map(reservationOutputDto =>
                new ReservationResponse(
                    reservationOutputDto.Id,
                    reservationOutputDto.UserId,
                    reservationOutputDto.ShowtimeId,
                    reservationOutputDto.SessionDateOnUtc,
                    reservationOutputDto.AuditoriumName,
                    reservationOutputDto.MovieTitle,
                    reservationOutputDto.Seats.ToResponse(),
                    reservationOutputDto.IsPurchased));
        }

        private static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<SeatOutputDto> seatOutputDtos)
        {
            ArgumentNullException.ThrowIfNull(seatOutputDtos);

            return ToResponseIterator(seatOutputDtos);
        }

        private static IEnumerable<SeatResponse> ToResponseIterator(
            IEnumerable<SeatOutputDto> seatOutputDtos)
        {
            foreach (var seatOutputDto in seatOutputDtos)
            {
                yield return new SeatResponse(
                    seatOutputDto.Id,
                    seatOutputDto.Row,
                    seatOutputDto.SeatNumber);
            }
        }
    }
}