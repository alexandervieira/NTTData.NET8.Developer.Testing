namespace Ambev.DeveloperEvoluation.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
