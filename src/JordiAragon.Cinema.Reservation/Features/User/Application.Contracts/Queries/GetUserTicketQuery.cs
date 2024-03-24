namespace JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetUserTicketQuery(Guid UserId, Guid ShowtimeId, Guid TicketId) : IQuery<TicketReadModel>;
}