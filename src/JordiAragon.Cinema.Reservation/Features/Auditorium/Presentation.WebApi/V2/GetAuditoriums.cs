namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class GetAuditoriums : EndpointWithoutRequest<IEnumerable<AuditoriumResponse>>
    {
        public const string Route = "auditoriums";

        private readonly ISender sender;
        private readonly IMapper mapper;

        public GetAuditoriums(ISender sender, IMapper mapper)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetAuditoriums.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Auditoriums";
                summary.Description = "Gets a list of all Auditoriums";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new GetAuditoriumsQuery(), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<AuditoriumResponse>>>(resultOutputDto);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}