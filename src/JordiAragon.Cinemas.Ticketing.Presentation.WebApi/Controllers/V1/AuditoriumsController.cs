namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Auditorium.Queries;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Commands;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Queries;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Controllers;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiVersion("1.0", Deprecated = true)]
    public class AuditoriumsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditoriumResponse>>> GetAsync(CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.Sender.Send(new GetAuditoriumsQuery(), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<AuditoriumResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }

        [HttpGet("{auditoriumId}/Showtimes")]
        public async Task<ActionResult<IEnumerable<ShowtimeResponse>>> GetShowtimesAsync(Guid auditoriumId, CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.Sender.Send(new GetShowtimesQuery(auditoriumId, MovieId: null, StartTimeOnUtc: null, EndTimeOnUtc: null), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<ShowtimeResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }

        // Create showtime.
        [HttpPost("{auditoriumId}/Showtimes")]
        public async Task<ActionResult<Guid>> CreateAsync(Guid auditoriumId, CreateShowtimeRequest request, CancellationToken cancellationToken)
        {
            var command = this.Mapper.Map<CreateShowtimeCommand>(request, opt =>
                        opt.AfterMap((_, command) =>
                        {
                            command.AuditoriumId = auditoriumId;
                        }));

            var result = await this.Sender.Send(command, cancellationToken);

            return this.ToActionResult(result);
        }

        // Reserve seats for a showtime.
        [HttpPost("{auditoriumId}/Showtimes/{showtimeId}/Tickets")]
        public async Task<ActionResult<TicketResponse>> CreateTicketAsync(Guid auditoriumId, Guid showtimeId, CreateTicketRequest request, CancellationToken cancellationToken)
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
        [HttpPatch("{auditoriumId}/Showtimes/{showtimeId}/Tickets/{ticketId}/Purchase")]
        public async Task<ActionResult> PurchaseTicketAsync(Guid auditoriumId, Guid showtimeId, Guid ticketId, CancellationToken cancellationToken)
        {
            var result = await this.Sender.Send(new PurchaseSeatsCommand(showtimeId, ticketId), cancellationToken);

            return this.ToActionResult(result);
        }

        // Get available seats.
        [HttpGet("{auditoriumId}/Showtimes/{showtimeId}/Seats/Available")]
        public async Task<ActionResult<IEnumerable<SeatResponse>>> GetAvailableSeatsAsync(Guid auditoriumId, Guid showtimeId, CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.Sender.Send(new GetAvailableSeatsQuery(showtimeId), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<SeatResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }
    }
}