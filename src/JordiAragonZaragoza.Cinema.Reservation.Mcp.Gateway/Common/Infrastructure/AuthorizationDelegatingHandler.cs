/*namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Context.Partition;

    public class AuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IPartitionContextService partitionContextService;
        private readonly IUserContextService userContextService;

        public AuthorizationDelegatingHandler(
            IPartitionContextService partitionContextService,
            IUserContextService userContextService)
        {
            this.partitionContextService = partitionContextService ?? throw new ArgumentNullException(nameof(partitionContextService));
            this.userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var userContext = this.userContextService.CurrentContext;
            request.Headers.Add(UserConstants.UserId, userContext.UserId);

            var partitionContext = this.partitionContextService.CurrentContext;

            request.Headers.Add(PartitionConstants.TenantId, partitionContext.TenantId);
            request.Headers.Add(PartitionConstants.ClientId, partitionContext.ClientId);

            return base.SendAsync(request, cancellationToken);
        }
    }
}*/