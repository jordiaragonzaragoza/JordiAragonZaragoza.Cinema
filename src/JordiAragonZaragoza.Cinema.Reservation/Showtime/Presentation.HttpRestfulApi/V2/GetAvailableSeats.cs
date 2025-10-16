namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class GetAvailableSeats : Endpoint<GetAvailableSeatsRequest, IEnumerable<SeatResponse>>
    {
        private readonly IQueryBus queryBus;

        public GetAvailableSeats(IQueryBus queryBus)
        {
            this.queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetAvailableSeatsRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets available Seats for an existing Showtime";
                summary.Description = "Gets available Seats for an existing Showtime";
            });
        }

        public async override Task HandleAsync(GetAvailableSeatsRequest req, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(req, nameof(req));

            var resultReadModels = await this.queryBus.SendAsync(new GetAvailableSeatsQuery(req.ShowtimeId), ct);

            var resultResponse = resultReadModels.ToResponse();

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}