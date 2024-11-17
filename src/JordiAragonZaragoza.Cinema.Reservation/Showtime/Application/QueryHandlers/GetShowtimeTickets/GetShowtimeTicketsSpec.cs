﻿namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeTickets
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetShowtimeTicketsSpec : Specification<TicketReadModel>, IPaginatedSpecification<TicketReadModel>
    {
        private readonly GetShowtimeTicketsQuery request;

        public GetShowtimeTicketsSpec(GetShowtimeTicketsQuery request)
        {
            this.request = Guard.Against.Null(request);

            this.Query
                .Where(ticket => ticket.ShowtimeId == request.ShowtimeId)
                .Include(ticket => ticket.Seats)
                .AsNoTracking();
        }

        public IPaginatedQuery Request
            => this.request;
    }
}