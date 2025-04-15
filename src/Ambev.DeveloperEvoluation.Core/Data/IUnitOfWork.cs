namespace Ambev.DeveloperEvoluation.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
        bool Commit();
        void Rollback();
    }
}
