namespace JordiAragonZaragoza.Cinema.Reservation.User.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class GetUserReservation : Endpoint<UserReservationRequest, ReservationResponse>
    {
        private readonly IQueryBus queryBus;

        public GetUserReservation(IQueryBus queryBus)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(UserReservationRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a user reservation for existing showtime";
                summary.Description = "Gets a user reservation for existing showtime";
            });
        }

        public override async Task HandleAsync(UserReservationRequest req, CancellationToken ct)
        {
            var resultReadModel = await this.queryBus.SendAsync(req.ToQuery(), ct);

            var resultResponse = resultReadModel.ToResponse();

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}