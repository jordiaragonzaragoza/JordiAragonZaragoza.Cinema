namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class CreateShowtime : Endpoint<CreateShowtimeRequest, Guid>
    {
        public const string Route = "showtimes";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public CreateShowtime(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Post(CreateShowtime.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Creates a new Showtime";
                summary.Description = "Creates a new Showtime";
            });
        }

        public async override Task HandleAsync(CreateShowtimeRequest req, CancellationToken ct)
        {
            var command = this.mapper.Map<CreateShowtimeCommand>(req);

            var resultResponse = await this.internalBus.Send(command, ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}