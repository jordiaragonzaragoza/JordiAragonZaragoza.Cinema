namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.Outbox
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.Outbox;
    using Microsoft.Extensions.Logging;
    using SharedKernelProcessOutboxMessagesJob = JordiAragon.SharedKernel.Infrastructure.Outbox.ProcessOutboxMessagesJob;

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