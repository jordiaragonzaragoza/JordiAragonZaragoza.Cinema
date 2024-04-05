namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public sealed class GetAvailableSeatsByShowtimeIdAndSeatIdsSpec : Specification<AvailableSeatReadModel>
    {
        public GetAvailableSeatsByShowtimeIdAndSeatIdsSpec(Guid showtimeId, IEnumerable<Guid> seatIds)
        {
            Guard.Against.Default(showtimeId);

            this.Query
                .Where(av => av.ShowtimeId == showtimeId && seatIds.Contains(av.SeatId))
                .AsNoTracking();
        }
    }
}