namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository;
        private readonly IExecutionContextService executionContextService;

        public AuthorizationService(
            ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> repository,
            IExecutionContextService executionContextService)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.executionContextService = executionContextService ?? throw new ArgumentNullException(nameof(executionContextService));
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

        public async Task<Result> AuthorizeAsync(
            ReadOnlyCollection<string> requiredRoles,
            ReadOnlyCollection<string> requiredPermissions,
            ReadOnlyCollection<string> requiredPolicies,
            CancellationToken cancellationToken = default)
        {
            var currentContext = this.executionContextService.CurrentContext;
            if (currentContext is null)
            {
                return Result.Unauthorized("No user is currently authenticated.");
            }

            if (currentContext.ActorType != ActorType.User)
            {
                // For service actors, we can implement a different authorization logic based on service identity and scopes.
                // This is a placeholder for service-to-service authorization logic.
                return Result.Success();
            }

            var userId = currentContext.GetUserActorId();
            var scopeContext = currentContext.ScopeContext;

            var query = new GetUserAuthorizationQuery(
                UserId: userId,
                TenantId: scopeContext.TenantId,
                PartitionId: scopeContext.PartitionId,
                CinemaId: scopeContext.DomainId);

            var userAuthorization = await this.repository.SingleOrDefaultAsync(new GetUserAuthorizationCachedSpecification(query), cancellationToken)
                ?? throw new InvalidOperationException($"User with ID {query.UserId} not found for tenant {query.TenantId}, partition {query.PartitionId}, and cinema {query.CinemaId}.");

            /*if (requiredPermissions.Except(userAuthorization.Permissions).Any())
            {
                return Result.Forbidden("User is missing required permissions for taking this action");
            }*/

            if (requiredRoles.Except(userAuthorization.Roles.Select(role => role.Value)).Any())
            {
                return Result.Forbidden("User is missing required roles for taking this action");
            }

            /*foreach (var policy in requiredPolicies)
            {
                var authorizationAgainstPolicyResult = _policyEnforcer.Authorize(request, currentUser, policy);

                if (authorizationAgainstPolicyResult.IsError)
                {
                    return authorizationAgainstPolicyResult.Errors;
                }
            }*/

            return Result.Success();
        }
    }
}