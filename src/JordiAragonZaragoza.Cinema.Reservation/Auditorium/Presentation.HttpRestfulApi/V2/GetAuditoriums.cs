namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    // TODO: It belongs to the management bounded context.
    public sealed class GetAuditoriums : Endpoint<GetAuditoriumsRequest, PaginatedCollectionResponse<AuditoriumResponse>>
    {
        private readonly IQueryBus queryBus;

        public GetAuditoriums(IQueryBus queryBus, IMapper mapper)
        {
            this.queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetAuditoriumsRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Auditoriums. Temporal: It belongs to the management bounded context";
                summary.Description = "Gets a list of all Auditoriums";
            });
        }

        public override async Task HandleAsync(GetAuditoriumsRequest req, CancellationToken ct)
        {
            var resultReadModel = await this.queryBus.SendAsync(req.ToQuery(), ct);

            var resultResponse = resultReadModel.ToResponse();
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}