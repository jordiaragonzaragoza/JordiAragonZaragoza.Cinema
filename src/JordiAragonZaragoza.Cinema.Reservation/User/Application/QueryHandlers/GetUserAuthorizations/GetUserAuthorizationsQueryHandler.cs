namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorizations
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class GetUserAuthorizationsQueryHandler : IQueryHandler<GetUserAuthorizationsQuery, IReadOnlyCollection<UserAuthorizationReadModel>>
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository;

        public GetUserAuthorizationsQueryHandler(
            ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Result<IReadOnlyCollection<UserAuthorizationReadModel>>> Handle(GetUserAuthorizationsQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            var specification = new GetUserAuthorizationsCandidatesCachedSpecification(
                query.UserId, query.TenantId);

            var results = await this.repository.ListAsync(specification, cancellationToken);

            if (results.Count == 0)
            {
                return Result.NotFound($"{nameof(UserAuthorizationReadModel)} not found.");
            }

            return Result.Success<IReadOnlyCollection<UserAuthorizationReadModel>>(results);
        }
    }
}
