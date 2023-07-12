namespace JordiAragon.Cinema.Application.Features.Auditorium.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ShowtimeDeletedNotificationHandler : INotificationHandler<ShowtimeDeletedNotification>
    {
        private readonly ILogger<ShowtimeDeletedNotificationHandler> logger;

        public ShowtimeDeletedNotificationHandler(
            ILogger<ShowtimeDeletedNotificationHandler> logger)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public Task Handle(ShowtimeDeletedNotification notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Handled Notification: {ApplicationEvent}", notification.GetType().Name);

            // TODO: Use some service to notify the admin.
            return Task.CompletedTask;
        }
    }
}