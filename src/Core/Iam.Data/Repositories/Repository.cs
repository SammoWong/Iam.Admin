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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ICommandRepository<TEntity> _commandRepository;
        private readonly IQueryRepository<TEntity> _queryRepository;
        public Repository(ICommandRepository<TEntity> commandRepository, IQueryRepository<TEntity> queryRepository)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.Any(predicate);
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.AnyAsync(predicate);
        }

        public virtual long Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.Count(predicate);
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.CountAsync(predicate);
        }

        public virtual void Delete(TEntity entity)
        {
            _commandRepository.Delete(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            _commandRepository.Delete(predicate);
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            return _commandRepository.DeleteAsync(entity);
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _commandRepository.DeleteAsync(predicate);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.FirstOrDefaultAsync(predicate);
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.Get(predicate);
        }

        public virtual Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.GetAsync(predicate);
        }

        public virtual void Insert(TEntity entity)
        {
            _commandRepository.Insert(entity);
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            return _commandRepository.InsertAsync(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            _commandRepository.InsertRange(entities);
        }

        public virtual Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            return _commandRepository.InsertRangeAsync(entities);
        }

        public virtual Task<PageableResponse<TEntity>> PagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false)
        {
            return _queryRepository.PagedAsync(pageIndex, pageSize, whereExpression, orderByExpression, ascending);
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _queryRepository.Query(predicate);
        }

        public virtual void Update(TEntity entity)
        {
            _commandRepository.Update(entity);
        }

        public virtual void Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> expression)
        {
            _commandRepository.Update(predicate, expression);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            return _commandRepository.UpdateAsync(entity);
        }

        public virtual Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> expression)
        {
            return _commandRepository.UpdateAsync(predicate, expression);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _commandRepository.UpdateRange(entities);
        }

        public virtual Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            return _commandRepository.UpdateRangeAsync(entities);
        }
    }
}
