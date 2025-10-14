namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    // TODO: It belongs to the cinema management bounded context.
    public sealed class GetMovies : Endpoint<GetMoviesRequest, PaginatedCollectionResponse<MovieResponse>>
    {
        private readonly IQueryBus queryBus;

        public GetMovies(IQueryBus queryBus)
        {
            this.queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetMoviesRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Movies. Temporal: It belongs to the cinema management bounded context.";
                summary.Description = "Gets a list of all Movies";
            });
        }

        public override async Task HandleAsync(GetMoviesRequest req, CancellationToken ct)
        {
            var resultReadModel = await this.queryBus.SendAsync(req.ToQuery(), ct);

            var resultResponse = resultReadModel.ToResponse();
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}