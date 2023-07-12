namespace JordiAragon.Cinema.Application.Features.Auditorium.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Events;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using Microsoft.Extensions.Logging;

    public class ShowtimeCreatedNotificationHandler : IDomainEventNotificationHandler<ShowtimeCreatedNotification>
    {
        private readonly ILogger<ShowtimeCreatedNotificationHandler> logger;

        public ShowtimeCreatedNotificationHandler(
            ILogger<ShowtimeCreatedNotificationHandler> logger)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public Task Handle(ShowtimeCreatedNotification notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Notification: {ApplicationEvent}", notification.GetType().Name);

            // TODO: Use some service to notify the admin.
            return Task.CompletedTask;
        }
    }
}