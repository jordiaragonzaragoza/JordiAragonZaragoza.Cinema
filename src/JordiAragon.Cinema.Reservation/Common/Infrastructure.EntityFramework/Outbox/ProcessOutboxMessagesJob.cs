namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Outbox
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using SharedKernelProcessOutboxMessagesJob = JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox.ProcessOutboxMessagesJob;

    public class ProcessOutboxMessagesJob : SharedKernelProcessOutboxMessagesJob
    {
        public ProcessOutboxMessagesJob(
            IDateTime dateTime,
            IPublisher internalBus,
            ILogger<ProcessOutboxMessagesJob> logger,
            ICachedSpecificationRepository<OutboxMessage, Guid> repositoryOutboxMessages)
            : base(dateTime, internalBus, logger, repositoryOutboxMessages)
        {
        }

        // This property is required due to a OutboxMessage deserialization.
        protected override IEnumerable<Assembly> CurrentAssemblies
            => new List<Assembly>() { AssemblyReference.Assembly };
    }
}