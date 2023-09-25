namespace JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework.Outbox
{
    using System.Collections.Generic;
    using System.Reflection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox;
    using JordiAragon.Cinemas.Ticketing.Domain;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using ApplicationContractsAssemblyReference = JordiAragon.Cinemas.Ticketing.Application.Contracts.ApplicationContractsAssemblyReference;
    using SharedKernelProcessOutboxMessagesJob = JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox.ProcessOutboxMessagesJob;

    public class ProcessOutboxMessagesJob : SharedKernelProcessOutboxMessagesJob
    {
        public ProcessOutboxMessagesJob(
            IDateTime dateTime,
            IPublisher mediator,
            ILogger<ProcessOutboxMessagesJob> logger,
            ICachedRepository<OutboxMessage> repositoryOutboxMessages)
            : base(dateTime, mediator, logger, repositoryOutboxMessages)
        {
        }

        protected override IEnumerable<Assembly> CurrentAssemblies
            => new List<Assembly>() { DomainAssemblyReference.Assembly, ApplicationContractsAssemblyReference.Assembly };
    }
}