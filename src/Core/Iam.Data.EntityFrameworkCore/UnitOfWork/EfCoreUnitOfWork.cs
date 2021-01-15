using Iam.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Iam.Data.EntityFrameworkCore.UnitOfWork
{
    public class EfCoreUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private IDbContextTransaction _dbTransaction;

        public EfCoreUnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void BeginTransaction()
        {
            _dbTransaction = _dbContext.Database.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _dbTransaction = _dbContext.Database.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            try
            {
                _dbTransaction?.Commit();
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            _dbTransaction?.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            _dbTransaction?.Dispose();
        }
    }
}
