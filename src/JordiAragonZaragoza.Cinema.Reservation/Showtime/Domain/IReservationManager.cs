namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;

    // TODO: Review. This interface probably is not required. Inject implementation instead.
    public interface IReservationManager
    {
        Task<Ticket> ReserveSeatsAsync(
            Showtime showtime,
            IEnumerable<SeatId> desiredSeatIds,
            TicketId newTicketId,
            UserId userId,
            ReservationDate reservationDateOnUtc,
            CancellationToken cancellationToken);

        Task<bool> HasShowtimeEndedAsync(Showtime showtime, DateTimeOffset currentDateTimeOnUtc, CancellationToken cancellationToken);
    }
}