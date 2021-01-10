using System;
using System.Data;

namespace Iam.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void Commit();

        void Rollback();
    }
}
