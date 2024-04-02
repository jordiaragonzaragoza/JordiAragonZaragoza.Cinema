namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public sealed class GetAvailableSeats : Endpoint<GetAvailableSeatsRequest, IEnumerable<SeatResponse>>
    {
        public const string Route = "showtimes/{showtimeId}/seats/available";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public GetAvailableSeats(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
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
            var resultReadModels = await this.internalBus.Send(new GetAvailableSeatsQuery(req.ShowtimeId), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<SeatResponse>>>(resultReadModels);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}