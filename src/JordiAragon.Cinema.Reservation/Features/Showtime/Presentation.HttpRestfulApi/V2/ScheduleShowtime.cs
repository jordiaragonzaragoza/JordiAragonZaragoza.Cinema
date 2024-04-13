namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public sealed class ScheduleShowtime : Endpoint<ScheduleShowtimeRequest, Guid>
    {
        public const string Route = "showtimes";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public ScheduleShowtime(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Post(ScheduleShowtime.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Creates a new Showtime";
                summary.Description = "Creates a new Showtime";
            });
        }

        public async override Task HandleAsync(ScheduleShowtimeRequest req, CancellationToken ct)
        {
            var command = this.mapper.Map<ScheduleShowtimeCommand>(req);

            var resultResponse = await this.internalBus.Send(command, ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}