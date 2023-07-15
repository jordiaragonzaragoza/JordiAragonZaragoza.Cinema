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
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous]
    [ApiVersion("2.0", Deprecated = false)]
    public class ShowtimesController : BaseApiController
    {
        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets a list of all Showtimes",
            Description = "Gets a list of all Showtimes",
            OperationId = "Showtime.List")
        ]
        public async Task<ActionResult<IEnumerable<ShowtimeResponse>>> GetShowtimesAsync(
            [FromQuery] Guid auditoriumId,
            CancellationToken cancellationToken,
            [FromQuery] Guid? movieId = null,
            [FromQuery] DateTime? startTimeOnUtc = null,
            [FromQuery] DateTime? endTimeOnUtc = null)
        {
            var resultOutputDto = await this.Sender.Send(new GetShowtimesQuery(auditoriumId, movieId, startTimeOnUtc, endTimeOnUtc), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<ShowtimeResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Showtime",
            Description = "Creates a new Showtime",
            OperationId = "Showtime.Create")
        ]
        public async Task<ActionResult<Guid>> CreateAsync(CreateShowtimeRequest request, CancellationToken cancellationToken)
        {
            var command = this.Mapper.Map<CreateShowtimeCommand>(request);

            var result = await this.Sender.Send(command, cancellationToken);

            return this.ToActionResult(result);
        }

        [HttpDelete("{showtimeId}")]
        [SwaggerOperation(
            Summary = "Deletes an existing Showtime",
            Description = "Deletes an existing Showtime",
            OperationId = "Showtime.Delete")
        ]
        public async Task<ActionResult<Guid>> DeleteAsync(Guid showtimeId, CancellationToken cancellationToken)
        {
            var result = await this.Sender.Send(new DeleteShowtimeCommand(showtimeId), cancellationToken);

            return this.ToActionResult(result);
        }

        [HttpGet("{showtimeId}/Seats/Available")]
        [SwaggerOperation(
            Summary = "Gets available Seats for an existing Showtime",
            Description = "Gets available Seats for an existing Showtime",
            OperationId = "Showtime.GetAvailableSeats")
        ]
        public async Task<ActionResult<IEnumerable<SeatResponse>>> GetAvailableSeatsAsync(Guid showtimeId, CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.Sender.Send(new GetAvailableSeatsQuery(showtimeId), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<SeatResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }

        [HttpPost("{showtimeId}/Tickets")]
        [SwaggerOperation(
            Summary = "Reserve Seats for an existing Showtime",
            Description = "Creates a Ticket for an existing Showtime",
            OperationId = "Showtime.CreateTicket")
        ]
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

        [HttpPatch("{showtimeId}/Tickets/{ticketId}/Purchase")]
        [SwaggerOperation(
            Summary = "Purchase a reservation for an existing Showtime",
            Description = "Purchase a Ticket for an existing Showtime",
            OperationId = "Showtime.PurchaseTicket")
        ]
        public async Task<ActionResult> PurchaseTicketAsync(Guid showtimeId, Guid ticketId, CancellationToken cancellationToken)
        {
            var result = await this.Sender.Send(new PurchaseSeatsCommand(showtimeId, ticketId), cancellationToken);

            return this.ToActionResult(result);
        }
    }
}