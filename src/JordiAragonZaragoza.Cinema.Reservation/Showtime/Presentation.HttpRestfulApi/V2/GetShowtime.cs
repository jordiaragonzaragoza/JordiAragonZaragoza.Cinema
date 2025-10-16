namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class GetShowtime : Endpoint<GetShowtimeRequest, ShowtimeResponse>
    {
        private readonly IQueryBus queryBus;

        public GetShowtime(IQueryBus queryBus)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetShowtimeRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Get an existing showtime";
                summary.Description = "Get an existing showtime";
            });
        }

        public async override Task HandleAsync(GetShowtimeRequest req, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(req);

            var resultReadModel = await this.queryBus.SendAsync(new GetShowtimeQuery(req.ShowtimeId), ct);

            var resultResponse = resultReadModel.ToResponse();

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}