﻿namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class ReserveSeats : Endpoint<ReserveSeatsRequest, ReservationResponse>
    {
        public const string Route = "showtimes/{showtimeId}/reservations/{reservationId}";

        private readonly ICommandBus commandBus;
        private readonly IMapper mapper;

        public ReserveSeats(ICommandBus commandBus, IMapper mapper)
        {
            this.commandBus = Guard.Against.Null(commandBus, nameof(commandBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Put(ReserveSeats.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Reserve Seats for an existing Showtime";
                summary.Description = "Reserve Seats for an existing Showtime";
            });
        }

        public async override Task HandleAsync(ReserveSeatsRequest req, CancellationToken ct)
        {
            var command = this.mapper.Map<ReserveSeatsCommand>(req);

            var resultOutputDto = await this.commandBus.SendAsync(command, ct);

            var resultResponse = this.mapper.Map<Result<ReservationResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}