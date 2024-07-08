namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V1
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class ReserveSeats : Endpoint<ReserveSeatsRequest, TicketResponse>
    {
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
            this.Post("auditoriums/{auditoriumId}/showtimes/{showtimeId}/tickets");
            this.Version(1, 2);
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

            var resultResponse = this.mapper.Map<Result<TicketResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}