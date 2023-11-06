namespace JordiAragon.Cinemas.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class GetAvailableSeats : Endpoint<GetAvailableSeatsRequest, IEnumerable<ShowtimeResponse>>
    {
        public const string Route = "showtimes/{showtimeId}/seats/available";

        private readonly ISender sender;
        private readonly IMapper mapper;

        public GetAvailableSeats(ISender sender, IMapper mapper)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetAvailableSeats.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets available Seats for an existing Showtime";
                summary.Description = "Gets available Seats for an existing Showtime";
            });
        }

        public async override Task HandleAsync(GetAvailableSeatsRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new GetAvailableSeatsQuery(req.ShowtimeId), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<SeatResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}