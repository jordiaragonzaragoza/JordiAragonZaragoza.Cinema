namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    // TODO: It belongs to the management bounded context.
    public sealed class GetAuditoriums : EndpointWithoutRequest<IEnumerable<AuditoriumResponse>>
    {
        public const string Route = "auditoriums";

        private readonly IQueryBus queryBus;
        private readonly IMapper mapper;

        public GetAuditoriums(IQueryBus queryBus, IMapper mapper)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetAuditoriums.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Auditoriums. Temporal: It belongs to the management bounded context";
                summary.Description = "Gets a list of all Auditoriums";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var resultOutputDto = await this.queryBus.SendAsync(new GetAuditoriumsQuery(), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<AuditoriumResponse>>>(resultOutputDto);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}