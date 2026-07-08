namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    using ExecutionContext = JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces.ExecutionContext;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserAuthorizationResolver userAuthorizationResolver;
        private readonly IPolicyEnforcer policyEnforcer;
        private readonly IExecutionContextService executionContextService;

        public AuthorizationService(
            IExecutionContextService executionContextService,
            UserAuthorizationResolver userAuthorizationResolver,
            IPolicyEnforcer policyEnforcer)
        {
            this.executionContextService = executionContextService ?? throw new ArgumentNullException(nameof(executionContextService));
            this.userAuthorizationResolver = userAuthorizationResolver ?? throw new ArgumentNullException(nameof(userAuthorizationResolver));
            this.policyEnforcer = policyEnforcer ?? throw new ArgumentNullException(nameof(policyEnforcer));
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
            Guid? resourceId,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(requiredRoles);
            ArgumentNullException.ThrowIfNull(requiredPermissions);
            ArgumentNullException.ThrowIfNull(requiredPolicies);

            var currentContext = this.executionContextService.CurrentContext;
            if (currentContext is null)
            {
                return Result.Unauthorized("No user is currently authenticated.");
            }

            return currentContext.ActorType switch
            {
                var t when t == ActorType.User
                    => await this.AuthorizeUserAsync(
                        currentContext,
                        requiredRoles,
                        requiredPermissions,
                        requiredPolicies,
                        resourceId,
                        cancellationToken),

                // System/Worker: Internally initiated action.
                // The actor is your own system reacting to an event or executing
                // a scheduled task — it is trusted because the action is audited
                // completely via ExecutionContext
                var t when t == ActorType.System
                    => Result.Success(),

                // External: Call from outside the platform without a JWT.
                // Only arrives here from [AllowAnonymous] endpoints (webhooks, registration).
                // These endpoints should NOT be decorated with [Authorize] — if they arrive here,
                // it's an endpoint configuration error, not a legitimate use case.
                var t when t == ActorType.External
                    => Result.Forbidden(
                        "External actors cannot invoke operations that require " +
                        "authorization. Endpoints accepting external actors must " +
                        "not declare [Authorize] on their commands/queries."),

                _ => Result.Unauthorized($"Unknown ActorType '{currentContext.ActorType.Name}'."),
            };
        }

        private async Task<Result> AuthorizeUserAsync(
            ExecutionContext currentContext,
            ReadOnlyCollection<string> requiredRoles,
            ReadOnlyCollection<string> requiredPermissions,
            ReadOnlyCollection<string> requiredPolicies,
            Guid? resourceId,
            CancellationToken cancellationToken)
        {
            var userId = currentContext.GetUserActorId();
            var scopeContext = currentContext.ScopeContext;

            var userAuthorization = await this.userAuthorizationResolver.ResolveAsync(
                userId, scopeContext, cancellationToken);

            if (userAuthorization is null)
            {
                return Result.NotFound(
                    $"User with ID {userId} not found for tenant {scopeContext.TenantId}, " +
                    $"partition {scopeContext.PartitionId}, and cinema {scopeContext.DomainId}.");
            }

            if (requiredPermissions.Except(userAuthorization.Permissions.Select(p => p.Value)).Any())
            {
                return Result.Forbidden("User is missing required permissions for taking this action");
            }

            if (requiredRoles.Except(userAuthorization.Roles.Select(r => r.Value)).Any())
            {
                return Result.Forbidden("User is missing required roles for taking this action");
            }

            var currentUserRoles = userAuthorization.Roles.Select(r => r.Value).ToList();
            var currentUserPermissions = userAuthorization.Permissions.Select(p => p.Value).ToList();

            foreach (var policy in requiredPolicies)
            {
                var policyResult = await this.policyEnforcer.AuthorizeAsync(
                    policy, userId, currentUserRoles, currentUserPermissions, resourceId, cancellationToken);

                if (!policyResult.IsSuccess)
                {
                    return policyResult;
                }
            }

            return Result.Success();
        }
    }
}