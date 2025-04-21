using Microsoft.EntityFrameworkCore.Storage;

namespace Ambev.DeveloperEvoluation.Core.Data
{
    public class UnitOfWorkTransaction : IUnitOfWork
    {
        private readonly IDbContextTransaction _transaction;

        public UnitOfWorkTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public Task<bool> CommitAsync()
        {
            throw new NotImplementedException();
        }       

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
  
}
