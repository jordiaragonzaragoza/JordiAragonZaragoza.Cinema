namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservation
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;

    public sealed class GetUserReservationSpecification : SingleResultSpecification<ReservationReadModel>
    {
        public GetUserReservationSpecification(GetUserReservationQuery request)
        {
            ArgumentNullException.ThrowIfNull(request);

            this.Query
                .Where(reservation => reservation.UserId == request.UserId && reservation.Id == request.ReservationId)
                .Include(reservation => reservation.Seats)
                .AsNoTracking();
        }
    }
}