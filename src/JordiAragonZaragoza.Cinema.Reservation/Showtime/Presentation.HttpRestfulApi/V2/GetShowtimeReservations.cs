namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class GetShowtimeReservations : Endpoint<GetShowtimeReservationsRequest, PaginatedCollectionResponse<ReservationResponse>>
    {
        private readonly IQueryBus queryBus;

        public GetShowtimeReservations(IQueryBus queryBus)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetShowtimeReservationsRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of reservations for an exiting showtime";
                summary.Description = "Gets a list of reservations for an exiting showtime";
            });
        }

        public async override Task HandleAsync(GetShowtimeReservationsRequest req, CancellationToken ct)
        {
            var resultReadModel = await this.queryBus.SendAsync(req.ToQuery(), ct);

            var resultResponse = resultReadModel.ToResponse();

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}