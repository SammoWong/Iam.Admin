using Iam.Core.ApiModels;
using Iam.Data.Entities;
using Iam.Data.MongoDB.Context;
using Iam.Data.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.MongoDB.Repositories
{
    public class MongoQueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : MongoEntity
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly IMongoQueryable<TEntity> _queryable;

        public MongoQueryRepository(IMongoContext context)
        {
            _collection = context.Database.GetCollection<TEntity>(typeof(TEntity).Name);
            _queryable = _collection.AsQueryable();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryable.Where(predicate).Any();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryable.Where(predicate).AnyAsync();
        }

        public long Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryable.Where(predicate).LongCount();
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryable.Where(predicate).LongCountAsync();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryable.FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryable.FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryable.Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _queryable.ToListAsync().ConfigureAwait(false);
        }

        public async Task<PageableResponse<TEntity>> PagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false)
        {
            var query = _queryable.Where(whereExpression);
            var total = await query.CountAsync();
            if (total == 0)
                return new PageableResponse<TEntity> { PageIndex = pageIndex, PageSize = pageSize };

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

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null) => _queryable.Where(predicate);
    }
}
