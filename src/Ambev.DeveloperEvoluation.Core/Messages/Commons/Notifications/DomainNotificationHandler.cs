using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvoluation.Core.Messages.Commons.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private readonly List<DomainNotification> _notifications;
        private readonly ILogger<DomainNotificationHandler> _logger;

        public DomainNotificationHandler(ILogger<DomainNotificationHandler> logger)
        {
            _notifications = new List<DomainNotification>();
            _logger = logger;
        }

        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            _notifications.Add(message);
            _logger.LogInformation("DomainNotification received: {Key} - {Value}", message.Key, message.Value);
            return Task.CompletedTask;
        }

        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public virtual bool TemNotificacao()
        {
            return GetNotifications().Any();
        }

        public void Dispose()
        {
            _notifications.Clear();
        }
    }
}
