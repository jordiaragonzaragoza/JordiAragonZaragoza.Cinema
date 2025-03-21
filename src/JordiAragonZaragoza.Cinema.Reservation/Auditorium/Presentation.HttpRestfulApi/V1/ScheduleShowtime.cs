namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V1
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class ScheduleShowtime : Endpoint<ScheduleShowtimeRequest, Guid>
    {
        private readonly ICommandBus commandBus;

        public ScheduleShowtime(ICommandBus commandBus)
        {
            this.commandBus = Guard.Against.Null(commandBus, nameof(commandBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Post("auditoriums/{auditoriumId}/showtimes");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Schedule a new Showtime";
                summary.Description = "Schedule a new Showtime";
            });
        }

        public async override Task HandleAsync(ScheduleShowtimeRequest req, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(req, nameof(req));

            // This generated Id is done here to support compatibility with the current implementation which client provides the Id.
            var showtimeId = Guid.NewGuid();

            var command = new ScheduleShowtimeCommand(
                showtimeId,
                AuditoriumId: req.AuditoriumId,
                MovieId: req.MovieId,
                SessionDateOnUtc: req.SessionDateOnUtc);

            var resultResponse = await this.commandBus.SendAsync(command, ct);
            if (resultResponse.IsSuccess)
            {
                await this.SendResponseAsync(Result.Created(showtimeId), ct);

                return;
            }

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}