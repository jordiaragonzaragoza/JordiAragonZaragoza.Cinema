namespace JordiAragon.Cinemas.Ticketing.Auditorium.Presentation.WebApi.V1
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Responses;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
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
            this.AllowAnonymous();
            this.Get("auditoriums/{auditoriumId}/showtimes");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Showtimes";
                summary.Description = "Gets a list of all Showtimes";
            });
        }

        public async override Task HandleAsync(GetShowtimesRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new GetShowtimesQuery(req.AuditoriumId, MovieId: null, StartTimeOnUtc: null, EndTimeOnUtc: null), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<ShowtimeResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}