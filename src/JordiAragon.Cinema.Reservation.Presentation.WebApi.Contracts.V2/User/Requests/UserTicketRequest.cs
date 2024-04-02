namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests
{
    using System;

    public sealed record class UserTicketRequest(Guid UserId, Guid ShowtimeId, Guid TicketId);
}