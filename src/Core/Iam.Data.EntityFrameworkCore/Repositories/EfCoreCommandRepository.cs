using Iam.Data.Entities;
using Iam.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Iam.Data.EntityFrameworkCore.Repositories
{
    public class EfCoreCommandRepository<TDbContext, TEntity> : ICommandRepository<TEntity> where TEntity : class, IEntity where TDbContext : DbContext
    {
        protected TDbContext DbContext { get; }
        public EfCoreCommandRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            DbContext.SaveChanges();
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            DbSet.Where(predicate).Delete();
            DbContext.SaveChanges();
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            return Task.Run(() => Delete(entity));
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).DeleteAsync();
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
            DbContext.SaveChanges();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            DbContext.SaveChanges();
        }

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
            await DbContext.SaveChangesAsync();
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
            DbContext.SaveChanges();
        }

        /// <summary>
        /// 按需要只更新部分更新
        /// 如：Update(u =>u.Id==1,u =>new User{Name="ok"});
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="expression"></param>
        public virtual void Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> expression)
        {
            DbSet.Where(predicate).Update(expression);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            return Task.Run(() => Update(entity));
        }

        public virtual Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> expression)
        {
            return DbSet.Where(predicate).UpdateAsync(expression);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            DbContext.SaveChanges();
        }

        public virtual Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            return Task.Run(() => UpdateRange(entities));
        }
    }
}
