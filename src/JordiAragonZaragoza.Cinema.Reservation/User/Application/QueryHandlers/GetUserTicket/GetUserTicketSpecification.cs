﻿namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserTicket
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;

    public sealed class GetUserTicketSpecification : SingleResultSpecification<TicketReadModel>
    {
        public GetUserTicketSpecification(GetUserTicketQuery request)
        {
            ArgumentNullException.ThrowIfNull(request);

            this.Query
                .Where(ticket => ticket.UserId == request.UserId && ticket.Id == request.TicketId)
                .Include(ticket => ticket.Seats)
                .AsNoTracking();
        }
    }
}