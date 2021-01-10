using Iam.Data.Entities;

namespace Iam.Data.Repositories
{
    public interface IRepository<TEntity> : IQueryRepository<TEntity>, ICommandRepository<TEntity> where TEntity : class, IEntity
    {
    }
}
