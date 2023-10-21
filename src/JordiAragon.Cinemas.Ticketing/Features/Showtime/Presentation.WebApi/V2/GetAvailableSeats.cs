namespace JordiAragon.Cinemas.Ticketing.Showtime.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Common.Presentation.WebApi;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Queries;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class GetAvailableSeats : Endpoint<GetAvailableSeatsRequest, IEnumerable<ShowtimeResponse>>
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public GetAvailableSeats(ISender sender, IMapper mapper)
        {
            this.sender = sender;
            this.mapper = mapper;
        }

        public override void Configure()
        {
            this.Get("showtimes/{showtimeId}/seats/available");
            this.AllowAnonymous();
            this.Version(2);
        }

        public async override Task HandleAsync(GetAvailableSeatsRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new GetAvailableSeatsQuery(req.ShowtimeId), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<SeatResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}