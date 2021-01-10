using Iam.Core.ApiModels;
using Iam.Data.Entities;
using Iam.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.EntityFrameworkCore.Repositories
{
    public class EfCoreQueryRepository<TDbContext, TEntity> : IQueryRepository<TEntity> where TEntity : class, IEntity where TDbContext : DbContext
    {
        protected virtual TDbContext DbContext { get; }

        public EfCoreQueryRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = DbContext.Set<TEntity>().AsNoTracking();
            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Query(predicate).Any();
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Query(predicate).AnyAsync();
        }

        public virtual long Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Query(predicate).LongCount();
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Query(predicate).LongCountAsync();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Query(predicate).FirstOrDefault();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Query(predicate).FirstOrDefaultAsync();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Query(predicate).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await Query(predicate).ToListAsync();
        }

        public virtual async Task<PageableResponse<TEntity>> PagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false)
        {
            var query = Query(whereExpression);
            var total = await query.CountAsync();
            if (total == 0)
                return new PageableResponse<TEntity> { PageIndex = pageIndex, PageSize = pageSize};

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            query = ascending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
            var data = await query.Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return new PageableResponse<TEntity>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                Items = data
            };
        }

    }
}
