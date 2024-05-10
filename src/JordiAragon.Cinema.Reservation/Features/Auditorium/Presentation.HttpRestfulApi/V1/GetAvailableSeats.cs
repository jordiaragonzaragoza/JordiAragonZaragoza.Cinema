namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V1
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public sealed class GetAvailableSeats : Endpoint<GetAvailableSeatsRequest, IEnumerable<SeatResponse>>
    {
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
            this.Get("auditoriums/{auditoriumId}/showtimes/{showtimeId}/seats/available");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets available Seats for an existing Showtime";
                summary.Description = "Gets available Seats for an existing Showtime";
            });
        }

        public async override Task HandleAsync(GetAvailableSeatsRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.internalBus.Send(new GetAvailableSeatsQuery(req.ShowtimeId), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<SeatResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}