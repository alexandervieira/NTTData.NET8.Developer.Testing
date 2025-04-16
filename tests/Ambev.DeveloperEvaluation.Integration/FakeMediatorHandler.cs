using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.Messages;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.DomainEvents;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.Notifications;
using MediatR;

public class FakeMediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;
    public List<Event> PublishedEvents { get; } = new();

    public FakeMediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> SendCommand<T>(T command) where T : Command
    {
        return await _mediator.Send(command);
    }

    public async Task PublishEvent<T>(T @event) where T : Event
    {
        PublishedEvents.Add(@event);
        await _mediator.Publish(@event);
    }

    public async Task PublishNotification<T>(T notification) where T : DomainNotification
    {
        await _mediator.Publish(notification);
    }

    public async Task PublishDomainEvent<T>(T notification) where T : DomainEvent
    {
        await _mediator.Publish(notification);
    }
}
