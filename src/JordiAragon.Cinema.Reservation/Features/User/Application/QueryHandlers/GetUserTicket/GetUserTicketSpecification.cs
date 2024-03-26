namespace JordiAragon.Cinema.Reservation.User.Application.QueryHandlers.GetUserTicket
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;

    public class GetUserTicketSpecification : SingleResultSpecification<TicketReadModel>
    {
        public GetUserTicketSpecification(GetUserTicketQuery request)
        {
            Guard.Against.Null(request);

            this.Query
                .Where(t => t.UserId == request.UserId && t.Id == request.TicketId);
        }
    }
}
