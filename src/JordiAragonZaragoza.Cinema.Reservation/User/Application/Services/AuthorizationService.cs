namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserAuthorizationResolver userAuthorizationResolver;

        private readonly IExecutionContextService executionContextService;

        public AuthorizationService(
            IExecutionContextService executionContextService,
            UserAuthorizationResolver userAuthorizationResolver)
        {
            this.executionContextService = executionContextService ?? throw new ArgumentNullException(nameof(executionContextService));
            this.userAuthorizationResolver = userAuthorizationResolver ?? throw new ArgumentNullException(nameof(userAuthorizationResolver));
        }

        public async Task<Result> ValidateScopeAsync(Guid userId, ScopeContext scope, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(scope);

            var userAuthorization = await this.userAuthorizationResolver.ResolveAsync(userId, scope, cancellationToken);
            if (userAuthorization is null)
            {
                return Result.NotFound($"User with ID {userId} not found for tenant {scope.TenantId}, partition {scope.PartitionId}, and cinema {scope.DomainId}.");
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

            var userAuthorization = await this.userAuthorizationResolver.ResolveAsync(userId, scopeContext, cancellationToken);
            if (userAuthorization is null)
            {
                return Result.NotFound($"User with ID {userId} not found for tenant {scopeContext.TenantId}, partition {scopeContext.PartitionId}, and cinema {scopeContext.DomainId}.");
            }

            if (requiredPermissions.Except(userAuthorization.Permissions.Select(permission => permission.Value)).Any())
            {
                return Result.Forbidden("User is missing required permissions for taking this action");
            }

            if (requiredRoles.Except(userAuthorization.Roles.Select(role => role.Value)).Any())
            {
                return Result.Forbidden("User is missing required roles for taking this action");
            }

            // TODO: Complete.
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