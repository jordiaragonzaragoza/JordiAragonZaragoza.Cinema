namespace JordiAragon.Cinema.Reservation.User.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Responses;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    using IMapper = AutoMapper.IMapper;

    // TODO: It belongs to the management bounded context.
    public sealed class GetUsers : EndpointWithoutRequest<IEnumerable<UserResponse>>
    {
        public const string Route = "users";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public GetUsers(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetUsers.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Users. Temporal: It belongs to the management bounded context.";
                summary.Description = "Gets a list of all Users";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var resultOutputDto = await this.internalBus.Send(new GetUsersQuery(), ct);

            var resultResponse = this.mapper.Map<Result<IEnumerable<UserResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}