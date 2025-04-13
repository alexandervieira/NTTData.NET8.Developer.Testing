using Ambev.DeveloperEvoluation.Core.DomainObjects;

namespace Ambev.DeveloperEvoluation.Core.Data
{
    public interface IRepository<T> : IDisposable where T : class, IAggregateRoot
    {
       IUnitOfWork UnitOfWork { get; }
    }
  
}
