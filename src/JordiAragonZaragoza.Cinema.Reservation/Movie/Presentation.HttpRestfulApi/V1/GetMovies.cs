namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Presentation.HttpRestfulApi.V1
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Movie.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    // TODO: It belongs to the catalog bounded context.
    public sealed class GetMovies : EndpointWithoutRequest<IEnumerable<MovieResponse>>
    {
        private readonly IQueryBus queryBus;

        public GetMovies(IQueryBus queryBus)
        {
            this.queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get("movies");
            this.Version(1);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Movies";
                summary.Description = "Gets a list of all Movies";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var resultReadModel = await this.queryBus.SendAsync(new GetMoviesQuery(PageNumber: 1, PageSize: 1), ct);

            var resultResponse = resultReadModel.ToResponse();
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}