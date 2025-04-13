using Ambev.DeveloperEvoluation.Core.Messages;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.DomainEvents;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.Notifications;
using MediatR;

namespace Ambev.DeveloperEvoluation.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        //public MediatorHandler()
        //{
        //}

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishDomainEvent<T>(T notification) where T : DomainEvent
        {
            await _mediator.Publish(notification);
        }

        public async Task PublishEvent<T>(T even) where T : Event
        {
            await _mediator.Publish(even);
        }

        public async Task PublishNotification<T>(T notification) where T : DomainNotification
        {
            await _mediator.Publish(notification);
        }

        public async Task<bool> SendCommand<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }
    }
}
