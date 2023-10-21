namespace JordiAragon.Cinemas.Ticketing.Auditorium.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Ticketing.Vertical.Common.Presentation.WebApi;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class GetAuditoriums : EndpointWithoutRequest<IEnumerable<AuditoriumResponse>>
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public GetAuditoriums(ISender sender, IMapper mapper)
        {
            this.sender = sender;
            this.mapper = mapper;
        }

        public override void Configure()
        {
            this.Get("auditoriums");
            this.AllowAnonymous();
            this.Version(2);
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new GetAuditoriumsQuery(), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<AuditoriumResponse>>>(resultOutputDto);
            ////this.Response = this.ToActionResult(resultResponse);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}