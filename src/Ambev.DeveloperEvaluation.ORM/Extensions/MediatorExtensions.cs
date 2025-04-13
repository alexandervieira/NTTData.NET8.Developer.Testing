using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;

namespace Ambev.DeveloperEvaluation.ORM.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task PublishEvents(this IMediatorHandler mediator, DefaultContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
