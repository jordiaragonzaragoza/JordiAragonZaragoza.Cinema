﻿namespace JordiAragon.Cinemas.Ticketing.Showtime.Presentation.WebApi.V2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class CreateShowtime : Endpoint<CreateShowtimeRequest, Guid>
    {
        public const string Route = "showtimes";

        private readonly ISender sender;
        private readonly IMapper mapper;

        public CreateShowtime(ISender sender, IMapper mapper)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
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

            var resultResponse = await this.sender.Send(command, ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}