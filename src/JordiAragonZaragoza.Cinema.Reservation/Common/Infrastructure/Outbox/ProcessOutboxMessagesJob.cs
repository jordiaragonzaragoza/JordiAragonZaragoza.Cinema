namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.Outbox
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Outbox;
    using Microsoft.Extensions.Logging;
    using SharedKernelProcessOutboxMessagesJob = JordiAragonZaragoza.SharedKernel.Infrastructure.Outbox.ProcessOutboxMessagesJob;

    public sealed class ProcessOutboxMessagesJob : SharedKernelProcessOutboxMessagesJob
    {
        public ProcessOutboxMessagesJob(
            IDateTime dateTime,
            IEventBus eventBus,
            ILogger<ProcessOutboxMessagesJob> logger,
            ICachedSpecificationRepository<OutboxMessage, Guid> repositoryOutboxMessages)
            : base(dateTime, eventBus, logger, repositoryOutboxMessages)
        {
        }

        // This property is required due to a OutboxMessage deserialization.
        protected override IEnumerable<Assembly> CurrentAssemblies
            => new List<Assembly>() { AssemblyReference.Assembly };
    }
}