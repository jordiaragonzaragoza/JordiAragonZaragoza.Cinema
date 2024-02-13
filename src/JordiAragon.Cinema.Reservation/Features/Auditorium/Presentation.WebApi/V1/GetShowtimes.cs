namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.WebApi.V1
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    public class GetShowtimes : Endpoint<GetShowtimesRequest, IEnumerable<ShowtimeResponse>>
    {
        private readonly ISender sender;

        public GetShowtimes(ISender sender)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get("auditoriums/{auditoriumId}/showtimes");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Showtimes";
                summary.Description = "Gets a list of all Showtimes";
            });
        }

        public async override Task HandleAsync(GetShowtimesRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new GetShowtimesQuery(req.AuditoriumId, MovieId: null, StartTimeOnUtc: null, EndTimeOnUtc: null, PageNumber: 1, PageSize: 0), ct);

            var resultResponse = MapToResponse(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }

        private static Result<IEnumerable<ShowtimeResponse>> MapToResponse(Result<PaginatedCollectionOutputDto<ShowtimeReadModel>> resultOutputDto)
        {
            Guard.Against.Null(resultOutputDto, nameof(resultOutputDto));

            if (!resultOutputDto.IsSuccess)
            {
                return Result<IEnumerable<ShowtimeResponse>>.Error(resultOutputDto.Errors.ToArray());
            }

            var paginatedCollection = resultOutputDto.Value;
            var showtimeResponses = paginatedCollection.Items
                .Select(showtime => new ShowtimeResponse(
                    showtime.Id,
                    showtime.MovieTitle,
                    showtime.SessionDateOnUtc,
                    showtime.AuditoriumId));

            return Result<IEnumerable<ShowtimeResponse>>.Success(showtimeResponses);
        }
    }
}