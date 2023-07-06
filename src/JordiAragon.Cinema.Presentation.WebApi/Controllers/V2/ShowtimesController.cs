namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V2
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Queries;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Controllers;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiVersion("2.0", Deprecated = false)]
    public class ShowtimesController : BaseApiController
    {
        // Get showtimes.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowtimeResponse>>> GetShowtimesAsync(
            [FromQuery] Guid auditoriumId,
            CancellationToken cancellationToken,
            [FromQuery] Guid? movieId = null,
            [FromQuery] DateTime? startTimeOnUtc = null,
            [FromQuery] DateTime? endTimeOnUtc = null)
        {
            var resultOutputDto = await this.Sender.Send(new GetShowtimesQuery(auditoriumId), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<ShowtimeResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }

        // Create showtime.
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAsync(CreateShowtimeRequest request, CancellationToken cancellationToken)
        {
            var command = this.Mapper.Map<CreateShowtimeCommand>(request);

            var result = await this.Sender.Send(command, cancellationToken);

            return this.ToActionResult(result);
        }

        // Delete showtime.
        [HttpDelete("{showtimeId}")]
        public async Task<ActionResult<Guid>> DeleteAsync(Guid showtimeId, CancellationToken cancellationToken)
        {
            var result = await this.Sender.Send(new DeleteShowtimeCommand(showtimeId), cancellationToken);

            return this.ToActionResult(result);
        }

        // Get available seats.
        [HttpGet("{showtimeId}/Seats/Available")]
        public async Task<ActionResult<IEnumerable<SeatResponse>>> GetAvailableSeatsAsync(Guid showtimeId, CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.Sender.Send(new GetAvailableSeatsQuery(showtimeId), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<SeatResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }

        // Reserve seats for a showtime.
        [HttpPost("{showtimeId}/Tickets")]
        public async Task<ActionResult<TicketResponse>> CreateTicketAsync(Guid showtimeId, CreateTicketRequest request, CancellationToken cancellationToken)
        {
            var command = this.Mapper.Map<ReserveSeatsCommand>(request, opt =>
                        opt.AfterMap((_, command) =>
                        {
                            command.ShowtimeId = showtimeId;
                        }));

            var resultOutputDto = await this.Sender.Send(command, cancellationToken);

            var resultResponse = this.Mapper.Map<Result<TicketResponse>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }

        // Purchase a reservation.
        [HttpPatch("{showtimeId}/Tickets/{ticketId}/Purchase")]
        public async Task<ActionResult> PurchaseTicketAsync(Guid showtimeId, Guid ticketId, CancellationToken cancellationToken)
        {
            var result = await this.Sender.Send(new PurchaseSeatsCommand(showtimeId, ticketId), cancellationToken);

            return this.ToActionResult(result);
        }
    }
}