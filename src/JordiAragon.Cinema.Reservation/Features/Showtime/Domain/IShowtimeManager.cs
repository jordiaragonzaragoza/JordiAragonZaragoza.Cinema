namespace JordiAragon.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.User.Domain;

    // TODO: Review. This interface probably is not required. Inject implementation instead.
    public interface IShowtimeManager
    {
        Task<Ticket> ReserveSeatsAsync(
            Showtime showtime,
            IEnumerable<SeatId> desiredSeatIds,
            TicketId newTicketId,
            UserId userId,
            DateTimeOffset currentDateTimeOnUtc,
            CancellationToken cancellationToken);

        Task<bool> HasShowtimeEndedAsync(Showtime showtime, DateTimeOffset currentDateTimeOnUtc, CancellationToken cancellationToken);
    }
}