namespace JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetUserTicketQuery(Guid UserId, Guid ShowtimeId, Guid TicketId) : IQuery<TicketReadModel>;
}