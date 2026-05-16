namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using ExecutionContext = JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces.ExecutionContext;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository;

        public AuthorizationService(ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Result> ValidateScopeAsync(ExecutionContext executionContext, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(executionContext);

            var scope = executionContext.ScopeContext;

            var query = new GetUserAuthorizationQuery(
                UserId: executionContext.GetUserActorId(),
                TenantId: scope.TenantId,
                PartitionId: scope.PartitionId,
                CinemaId: scope.DomainId);

            var readModel = await this.repository.SingleOrDefaultAsync(new GetUserAuthorizationCachedSpecification(query), cancellationToken);
            if (readModel is null)
            {
                return Result.NotFound($"User with ID {query.UserId} not found for tenant {query.TenantId}, partition {query.PartitionId}, and cinema {query.CinemaId}.");
            }

            return Result.Success();
        }

        public async Task<Result> ValidateScopeAsync(Guid userId, ScopeContext scope, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(scope);

            var query = new GetUserAuthorizationQuery(
                UserId: userId,
                TenantId: scope.TenantId,
                PartitionId: scope.PartitionId,
                CinemaId: scope.DomainId);

            var readModel = await this.repository.SingleOrDefaultAsync(new GetUserAuthorizationCachedSpecification(query), cancellationToken);
            if (readModel is null)
            {
                return Result.NotFound($"User with ID {query.UserId} not found for tenant {query.TenantId}, partition {query.PartitionId}, and cinema {query.CinemaId}.");
            }

            return Result.Success();
        }
    }
}