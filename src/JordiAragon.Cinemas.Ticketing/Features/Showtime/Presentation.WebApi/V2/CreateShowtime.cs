namespace JordiAragon.Cinemas.Ticketing.Showtime.Presentation.WebApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Common.Presentation.WebApi;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class CreateShowtime : Endpoint<CreateShowtimeRequest, Guid>
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public CreateShowtime(ISender sender, IMapper mapper)
        {
            this.sender = sender;
            this.mapper = mapper;
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Post("showtimes");
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

            var resultResponse = await this.sender.Send(command, ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}