namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class ReserveSeats : Endpoint<ReserveSeatsRequest, TicketResponse>
    {
        public const string Route = "showtimes/{showtimeId}/tickets";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public ReserveSeats(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Post(ReserveSeats.Route);
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

            var resultOutputDto = await this.internalBus.Send(command, ct);

            var resultResponse = this.mapper.Map<Result<TicketResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}