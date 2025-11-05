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

        /*
        // ReadModels to responses.
        public static Result<IEnumerable<AuditoriumResponse>> ToResponse(
            this Result<PaginatedCollectionOutputDto<AuditoriumReadModel>> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(paginatedCollection => paginatedCollection.Items.ToResponse());
        }

        public static Result<IEnumerable<SeatResponse>> ToResponse(
            this Result<IEnumerable<AvailableSeatReadModel>> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(availableSeats => availableSeats.ToResponse());
        }

        public static Result<IEnumerable<ShowtimeResponse>> ToResponse(
            this Result<PaginatedCollectionOutputDto<ShowtimeReadModel>> resultOutputDto)
        {
            ArgumentNullException.ThrowIfNull(resultOutputDto);

            return resultOutputDto.Map(paginatedCollection => paginatedCollection.Items.ToResponse());
        }

        private static IEnumerable<ShowtimeResponse> ToResponse(
            this IEnumerable<ShowtimeReadModel> showtimeReadModels)
        {
            ArgumentNullException.ThrowIfNull(showtimeReadModels);

            return ToResponseIterator(showtimeReadModels);
        }

        private static IEnumerable<AuditoriumResponse> ToResponse(
            this IEnumerable<AuditoriumReadModel> auditoriumReadModels)
        {
            ArgumentNullException.ThrowIfNull(auditoriumReadModels);

            return ToResponseIterator(auditoriumReadModels);
        }

        private static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<AvailableSeatReadModel> availableSeatReadModels)
        {
            ArgumentNullException.ThrowIfNull(availableSeatReadModels);

            return ToResponseIterator(availableSeatReadModels);
        }

        private static IEnumerable<SeatResponse> ToResponseIterator(
            this IEnumerable<AvailableSeatReadModel> availableSeatReadModels)
        {
            foreach (var availableSeatReadModel in availableSeatReadModels)
            {
                yield return new SeatResponse(
                    availableSeatReadModel.SeatId,
                    availableSeatReadModel.Row,
                    availableSeatReadModel.SeatNumber);
            }
        }

        private static IEnumerable<AuditoriumResponse> ToResponseIterator(
            IEnumerable<AuditoriumReadModel> auditoriumReadModels)
        {
            foreach (var auditoriumReadModel in auditoriumReadModels)
            {
                yield return new AuditoriumResponse(
                    auditoriumReadModel.Id,
                    auditoriumReadModel.Name);
            }
        }

        private static IEnumerable<ShowtimeResponse> ToResponseIterator(
            IEnumerable<ShowtimeReadModel> showtimeReadModels)
        {
            foreach (var showtimeReadModel in showtimeReadModels)
            {
                yield return new ShowtimeResponse(
                    showtimeReadModel.Id,
                    showtimeReadModel.MovieTitle,
                    showtimeReadModel.SessionDateOnUtc,
                    showtimeReadModel.AuditoriumId);
            }
        }

        */
    }
}