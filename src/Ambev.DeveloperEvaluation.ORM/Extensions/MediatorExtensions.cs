using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Extensions
{   
    public static class MediatorExtensions
    {
        public static async Task PublishEvents<TContext>(this IMediatorHandler mediator, TContext context)
            where TContext : DbContext
        {
            var domainEntities = context.ChangeTracker
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
