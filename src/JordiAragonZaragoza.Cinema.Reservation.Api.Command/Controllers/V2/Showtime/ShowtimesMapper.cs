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
        public static PurchaseReservationCommand ToCommand(this PurchaseReservationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new PurchaseReservationCommand(
                request.ShowtimeId,
                request.ReservationId);
        }

        public static ReserveSeatsCommand ToCommand(this ReserveSeatsRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new ReserveSeatsCommand(
                request.ReservationId,
                request.ShowtimeId,
                request.SeatsIds);
        }

        public static ScheduleShowtimeCommand ToCommand(this ScheduleShowtimeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new ScheduleShowtimeCommand(
                request.ShowtimeId,
                request.AuditoriumId,
                request.MovieId,
                request.SessionDateOnUtc);
        }

        public static CancelShowtimeCommand ToCommand(this CancelShowtimeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new CancelShowtimeCommand(request.ShowtimeId);
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

        /*
        public static GetShowtimesQuery ToQuery(this GetShowtimesRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GetShowtimesQuery(
                request.AuditoriumId,
                request.AuditoriumName,
                request.MovieId,
                request.MovieTitle,
                request.StartTimeOnUtc,
                request.EndTimeOnUtc,
                request.PageNumber ?? 1,
                request.PageSize ?? 10);
        }

        public static GetShowtimeReservationsQuery ToQuery(this GetShowtimeReservationsRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GetShowtimeReservationsQuery(
                request.ShowtimeId,
                request.PageNumber ?? 1,
                request.PageSize ?? 10);
        }

        public static Result<PaginatedCollectionResponse<ReservationResponse>> ToResponse(
            this Result<PaginatedCollectionOutputDto<ReservationReadModel>> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(paginatedCollection =>
                new PaginatedCollectionResponse<ReservationResponse>(
                    paginatedCollection.ActualPage,
                    paginatedCollection.TotalPages,
                    paginatedCollection.TotalItems,
                    paginatedCollection.Items.ToResponse()));
        }

        public static Result<IEnumerable<SeatResponse>> ToResponse(
            this Result<IEnumerable<AvailableSeatReadModel>> resultReadModels)
        {
            ArgumentNullException.ThrowIfNull(resultReadModels);

            return resultReadModels.Map(availableSeatReadModels => availableSeatReadModels.ToResponse());
        }

        public static Result<ShowtimeResponse> ToResponse(
            this Result<ShowtimeReadModel> resultReadModel)
        {
            ArgumentNullException.ThrowIfNull(resultReadModel);

            return resultReadModel.Map(showtimeReadModel =>
                new ShowtimeResponse(
                    showtimeReadModel.Id,
                    showtimeReadModel.MovieTitle,
                    showtimeReadModel.SessionDateOnUtc,
                    showtimeReadModel.AuditoriumId,
                    showtimeReadModel.AuditoriumName));
        }

        public static Result<PaginatedCollectionResponse<ShowtimeResponse>> ToResponse(
            this Result<PaginatedCollectionOutputDto<ShowtimeReadModel>> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.Map(paginatedCollection =>
                new PaginatedCollectionResponse<ShowtimeResponse>(
                    paginatedCollection.ActualPage,
                    paginatedCollection.TotalPages,
                    paginatedCollection.TotalItems,
                    paginatedCollection.Items.ToResponse()));
        }

        private static IEnumerable<ShowtimeResponse> ToResponse(
            this IEnumerable<ShowtimeReadModel> showtimeReadModels)
        {
            ArgumentNullException.ThrowIfNull(showtimeReadModels);

            return ToResponseIterator(showtimeReadModels);
        }

        private static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<AvailableSeatReadModel> availableSeatReadModels)
        {
            ArgumentNullException.ThrowIfNull(availableSeatReadModels);

            return ToResponseIterator(availableSeatReadModels);
        }

        private static IEnumerable<ReservationResponse> ToResponse(
            this IEnumerable<ReservationReadModel> reservationReadModels)
        {
            ArgumentNullException.ThrowIfNull(reservationReadModels);

            return ToResponseIterator(reservationReadModels);
        }

        private static IEnumerable<SeatResponse> ToResponse(
            this IEnumerable<SeatReadModel> seatReadModels)
        {
            ArgumentNullException.ThrowIfNull(seatReadModels);

            return ToResponseIterator(seatReadModels);
        }

        private static IEnumerable<SeatResponse> ToResponseIterator(
            IEnumerable<AvailableSeatReadModel> availableSeatReadModels)
        {
            foreach (var availableSeatReadModel in availableSeatReadModels)
            {
                yield return new SeatResponse(
                    availableSeatReadModel.Id,
                    availableSeatReadModel.Row,
                    availableSeatReadModel.SeatNumber);
            }
        }

        private static IEnumerable<ReservationResponse> ToResponseIterator(
            IEnumerable<ReservationReadModel> reservationReadModels)
        {
            foreach (var reservationReadModel in reservationReadModels)
            {
                yield return new ReservationResponse(
                    reservationReadModel.Id,
                    reservationReadModel.UserId,
                    reservationReadModel.ShowtimeId,
                    reservationReadModel.SessionDateOnUtc,
                    reservationReadModel.AuditoriumName,
                    reservationReadModel.MovieTitle,
                    reservationReadModel.Seats.ToResponse(),
                    reservationReadModel.IsPurchased);
            }
        }

        private static IEnumerable<SeatResponse> ToResponseIterator(
            IEnumerable<SeatReadModel> seatReadModels)
        {
            foreach (var seatReadModel in seatReadModels)
            {
                yield return new SeatResponse(
                    seatReadModel.Id,
                    seatReadModel.Row,
                    seatReadModel.SeatNumber);
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
                    showtimeReadModel.AuditoriumId,
                    showtimeReadModel.AuditoriumName);
            }
        }*/
    }
}