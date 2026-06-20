namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorizations;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public class UserAuthorizationResolver
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository;
        private (Guid UserId, ScopeContext Scope, UserAuthorizationReadModel? Result)? last;

        public UserAuthorizationResolver(ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Resolves the single assignment whose scope hierarchy covers the requested
        /// (tenantId, partitionId, cinemaId), using UserAuthorizationReadModel.Matches
        /// for the same Cinema > Partition > Tenant specificity rule the domain applies
        /// in User.GetRolesFor. A user is expected to have at most one matching
        /// assignment per concrete scope; if a user has multiple overlapping assignments
        /// (e.g. Tenant-level AND Cinema-level), the most specific one is preferred.
        /// </summary>
        /// <param name="userId">The ID of the user for whom to resolve authorization.</param>
        /// <param name="scope">The scope context (tenant, partition, cinema) for which to resolve authorization.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The resolved UserAuthorizationReadModel if found; otherwise, null.</returns>
        public async Task<UserAuthorizationReadModel?> ResolveAsync(
            Guid userId,
            ScopeContext scope,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(scope);

            if (this.last is { } cached && cached.UserId == userId && cached.Scope == scope)
            {
                return cached.Result;
            }

            var candidates = await this.repository.ListAsync(
                new GetUserAuthorizationsCandidatesCachedSpecification(userId, scope.TenantId), cancellationToken);

            var result = candidates
                .Where(c => c.Matches(scope.TenantId, scope.PartitionId, scope.DomainId))
                .OrderByDescending(GetSpecificity)
                .FirstOrDefault();

            static int GetSpecificity(UserAuthorizationReadModel authorization)
            {
                if (authorization.CinemaId is not null)
                {
                    return 2;
                }

                if (authorization.PartitionId is not null)
                {
                    return 1;
                }

                return 0;
            }

            this.last = (userId, scope, result);
            return result;
        }
    }
}