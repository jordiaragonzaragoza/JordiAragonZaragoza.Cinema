namespace JordiAragon.Cinema.Application.AssemblyConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class ReMediator : Mediator
    {
        public ReMediator(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        //// Unfortunately Publish() seems to hit the handlers twice!!!
        //// https://github.com/jbogard/MediatR/issues/702
        //// https://github.com/jbogard/MediatR/issues/718
        //// https://github.com/jbogard/MediatR.Extensions.Microsoft.DependencyInjection/issues/118
        protected override async Task PublishCore(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
        {
            var newHandlerExecutors = new List<NotificationHandlerExecutor>();
            foreach (var handler in handlerExecutors)
            {
                if (!newHandlerExecutors.Any(n => n.HandlerInstance.GetType() == handler.HandlerInstance.GetType()))
                {
                    newHandlerExecutors.Add(handler);
                }
            }

            await base.PublishCore(newHandlerExecutors, notification, cancellationToken);
        }
    }
}