namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class ScheduleShowtime : Endpoint<ScheduleShowtimeRequest, Guid>
    {
        public const string Route = "showtimes";

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
            this.Post(ScheduleShowtime.Route);
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