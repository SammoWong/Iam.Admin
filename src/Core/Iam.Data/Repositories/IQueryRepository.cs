using Iam.Core.ApiModels;
using Iam.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.Repositories
{
    public interface IQueryRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null);

        long Count(Expression<Func<TEntity, bool>> predicate = null);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null);

        bool Any(Expression<Func<TEntity, bool>> predicate = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<PageableResponse<TEntity>> PagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false);
    }
}
