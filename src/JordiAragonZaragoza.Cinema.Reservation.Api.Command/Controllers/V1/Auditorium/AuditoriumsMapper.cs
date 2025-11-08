namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V1.Auditorium
{
    using System;
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;

    public static class AuditoriumsMapper
    {
        // Requests to queries or commands.
        public static ScheduleShowtimeCommand ToCommand(this ScheduleShowtimeRequest request, Guid showtimeId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new ScheduleShowtimeCommand(
                showtimeId,
                AuditoriumId: request.AuditoriumId,
                MovieId: request.MovieId,
                SessionDateOnUtc: request.SessionDateOnUtc);
        }

        public static PurchaseReservationCommand ToCommand(this PurchaseReservationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new PurchaseReservationCommand(
                request.ShowtimeId,
                request.ReservationId);
        }

        public static ReserveSeatsCommand ToCommand(this ReserveSeatsRequest request, Guid reservationId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new ReserveSeatsCommand(
                reservationId,
                request.ShowtimeId,
                request.SeatsIds);
        }

        public static Result<ReservationResponse> ToResponse(
            this Result<ReservationOutputDto> resultOutputDto)
        {
            ArgumentNullException.ThrowIfNull(resultOutputDto);

            return resultOutputDto.Map(reservationOutputDto
                => new ReservationResponse(
                    reservationOutputDto.Id,
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