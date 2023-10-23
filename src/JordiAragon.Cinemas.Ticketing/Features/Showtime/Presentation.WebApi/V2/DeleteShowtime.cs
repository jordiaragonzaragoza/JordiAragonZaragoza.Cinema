namespace JordiAragon.Cinemas.Ticketing.Showtime.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    public class DeleteShowtime : Endpoint<DeleteShowtimeRequest>
    {
        private readonly ISender sender;

        public DeleteShowtime(ISender sender)
        {
            this.sender = sender;
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Delete("showtimes");
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Deletes an existing Showtime";
                summary.Description = "Deletes an existing Showtime";
            });
        }

        public async override Task HandleAsync(DeleteShowtimeRequest req, CancellationToken ct)
        {
            var resultResponse = await this.sender.Send(new DeleteShowtimeCommand(req.ShowtimeId), ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}