using Ambev.DeveloperEvoluation.Core.Messages;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.DomainEvents;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.Notifications;

namespace Ambev.DeveloperEvoluation.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T even) where T : Event;
        Task<bool> SendCommand<T>(T command) where T : Command;
        Task PublishNotification<T>(T notification) where T : DomainNotification;
        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;
    }
}
