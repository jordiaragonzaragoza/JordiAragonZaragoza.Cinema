﻿namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class ScheduleShowtime : Endpoint<ScheduleShowtimeRequest>
    {
        public const string Route = "showtimes/{showtimeId}";

        private readonly ICommandBus commandBus;
        private readonly IMapper mapper;

        public ScheduleShowtime(ICommandBus commandBus, IMapper mapper)
        {
            this.commandBus = Guard.Against.Null(commandBus, nameof(commandBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Put(ScheduleShowtime.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Schedule a new Showtime";
                summary.Description = "Schedule a new Showtime";
            });
        }

        public async override Task HandleAsync(ScheduleShowtimeRequest req, CancellationToken ct)
        {
            var command = this.mapper.Map<ScheduleShowtimeCommand>(req);

            var resultResponse = await this.commandBus.SendAsync(command, ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}