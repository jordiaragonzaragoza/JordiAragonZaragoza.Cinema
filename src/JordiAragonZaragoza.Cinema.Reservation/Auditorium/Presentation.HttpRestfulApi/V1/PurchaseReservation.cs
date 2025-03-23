﻿namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V1
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Reservation.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class PurchaseReservation : Endpoint<PurchaseReservationRequest, ReservationResponse>
    {
        private readonly ICommandBus commandBus;
        private readonly IMapper mapper;

        public PurchaseReservation(ICommandBus commandBus, IMapper mapper)
        {
            this.commandBus = Guard.Against.Null(commandBus, nameof(commandBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Patch("auditoriums/{auditoriumId}/showtimes/{showtimeId}/reservations/{reservationId}/Purchase");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Purchase a reservation for an existing Showtime. Temporal: This endpoint will not be exposed on finance integration. ";
                summary.Description = "Purchase a reservation for an existing Showtime";
            });
        }

        public async override Task HandleAsync(PurchaseReservationRequest req, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(req, nameof(req));

            var resultOutputDto = await this.commandBus.SendAsync(new PurchaseReservationCommand(req.ShowtimeId, req.ReservationId), ct);

            var resultResponse = this.mapper.Map<Result<ReservationResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}