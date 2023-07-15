namespace JordiAragon.Cinema.Domain.ShowtimeAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Rules;
    using JordiAragon.SharedKernel.Domain.Services;

    public class ShowtimeManager : BaseDomainService
    {
        public static IEnumerable<Seat> AvailableSeats(Auditorium auditorium, Showtime showtime)
        {
            var reservedSeats = ReservedSeats(auditorium, showtime);

            return auditorium.Seats.Except(reservedSeats)
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
        }

        public static Ticket ReserveSeats(
            Auditorium auditorium,
            Showtime showtime,
            IEnumerable<SeatId> desiredSeatIds,
            TicketId ticketId,
            DateTime createdTimeOnUtc)
        {
            var desiredSeats = auditorium.Seats.Where(seat => desiredSeatIds.Contains(seat.Id));

            CheckRule(new OnlyContiguousSeatsCanBeReservedRule(desiredSeats));

            var availableSeats = AvailableSeats(auditorium, showtime);

            CheckRule(new OnlyAvailableSeatsCanBeReservedRule(desiredSeats, availableSeats));

            return showtime.ReserveSeats(ticketId, desiredSeatIds, createdTimeOnUtc);
        }

        private static IEnumerable<Seat> ReservedSeats(Auditorium auditorium, Showtime showtime)
        {
            var seatIds = showtime.Tickets.SelectMany(ticket => ticket.Seats);

            return auditorium.Seats.Where(seat => seatIds.Contains(seat.Id))
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
        }
    }
}