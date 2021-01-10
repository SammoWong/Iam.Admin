using Iam.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.Repositories
{
    public interface ICommandRepository<TEntity> where TEntity : class, IEntity
    {
        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity);

        void InsertRange(IEnumerable<TEntity> entities);

        Task InsertRangeAsync(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        Task DeleteAsync(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> predicate);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity);

        void Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> expression);

        Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> expression);

        void UpdateRange(IEnumerable<TEntity> entities);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}
