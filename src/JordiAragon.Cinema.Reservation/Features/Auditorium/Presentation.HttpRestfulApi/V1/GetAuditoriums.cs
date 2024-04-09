namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V1
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Responses;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    // TODO: It belongs to the cinema manager bounded context.
    public sealed class GetAuditoriums : EndpointWithoutRequest<IEnumerable<AuditoriumResponse>>
    {
        public const string Route = "auditoriums";
        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public GetAuditoriums(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetAuditoriums.Route);
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Auditoriums";
                summary.Description = "Gets a list of all Auditoriums";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var resultOutputDto = await this.internalBus.Send(new GetAuditoriumsQuery(), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<AuditoriumResponse>>>(resultOutputDto);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}