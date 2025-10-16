namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class GetShowtimes : Endpoint<GetShowtimesRequest, PaginatedCollectionResponse<ShowtimeResponse>>
    {
        private readonly IQueryBus queryBus;

        public GetShowtimes(IQueryBus queryBus)
        {
            this.queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetShowtimesRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Showtimes";
                summary.Description = "Gets a list of all Showtimes";
            });
        }

        public async override Task HandleAsync(GetShowtimesRequest req, CancellationToken ct)
        {
            var resultReadModel = await this.queryBus.SendAsync(req.ToQuery(), ct);

            var resultResponse = resultReadModel.ToResponse();

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}