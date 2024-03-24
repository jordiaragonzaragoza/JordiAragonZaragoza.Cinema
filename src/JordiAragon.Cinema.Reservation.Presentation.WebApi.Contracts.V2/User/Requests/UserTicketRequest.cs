namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests
{
    using System;

    public record class UserTicketRequest(Guid UserId, Guid ShowtimeId, Guid TicketId);
}