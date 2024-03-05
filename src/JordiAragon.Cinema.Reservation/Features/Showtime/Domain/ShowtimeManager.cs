namespace JordiAragon.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragon.SharedKernel.Domain.Services;

    public class ShowtimeManager : BaseDomainService
    {
        public static Ticket ReserveSeats(
            Auditorium auditorium,
            Showtime showtime,
            IEnumerable<SeatId> desiredSeatIds,
            TicketId ticketId,
            DateTimeOffset createdTimeOnUtc)
        {
            Guard.Against.Null(auditorium);
            Guard.Against.Null(showtime);
            Guard.Against.NullOrEmpty(desiredSeatIds);
            Guard.Against.Null(ticketId);
            Guard.Against.Default(createdTimeOnUtc);

            var desiredSeats = auditorium.Seats.Where(seat => desiredSeatIds.Contains(seat.Id));

            CheckRule(new OnlyContiguousSeatsCanBeReservedRule(desiredSeats));

            var availableSeats = AvailableSeats(auditorium, showtime);

            CheckRule(new OnlyAvailableSeatsCanBeReservedRule(desiredSeats, availableSeats));

            return showtime.ReserveSeats(ticketId, desiredSeatIds, createdTimeOnUtc);
        }

        private static IEnumerable<Seat> AvailableSeats(Auditorium auditorium, Showtime showtime)
        {
            Guard.Against.Null(auditorium);
            Guard.Against.Null(showtime);

            var reservedSeats = ReservedSeats(auditorium, showtime);

            return auditorium.Seats.Except(reservedSeats)
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
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