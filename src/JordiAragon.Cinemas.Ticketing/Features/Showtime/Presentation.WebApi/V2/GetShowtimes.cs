namespace JordiAragon.Cinemas.Ticketing.Showtime.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Common.Presentation.WebApi;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Queries;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class GetShowtimes : Endpoint<GetShowtimesRequest, IEnumerable<ShowtimeResponse>>
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public GetShowtimes(ISender sender, IMapper mapper)
        {
            this.sender = sender;
            this.mapper = mapper;
        }

        public override void Configure()
        {
            this.Get("showtimes");
            this.AllowAnonymous();
            this.Version(2);
        }

        public async override Task HandleAsync(GetShowtimesRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new GetShowtimesQuery(req.AuditoriumId, req.MovieId, req.StartTimeOnUtc, req.EndTimeOnUtc), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<ShowtimeResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}