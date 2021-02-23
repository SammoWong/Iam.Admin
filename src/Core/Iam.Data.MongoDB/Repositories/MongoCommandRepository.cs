using Iam.Data.Entities;
using Iam.Data.MongoDB.Context;
using Iam.Data.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.MongoDB.Repositories
{
    public class MongoCommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : MongoEntity
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoCommandRepository(IMongoContext context)
        {
            _collection = context.Database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public void Delete(TEntity entity)
        {
            _collection.DeleteOne(Filters.IdEq<TEntity>(entity.Id));
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            _collection.DeleteMany(predicate);
        }

        public Task DeleteAsync(TEntity entity)
        {
            return _collection.DeleteManyAsync(Filters.IdEq<TEntity>(entity.Id));
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _collection.DeleteManyAsync(predicate);
        }

        public void Insert(TEntity entity)
        {
            _collection.InsertOne(entity);
        }

        public Task InsertAsync(TEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            _collection.InsertMany(entities);
        }

        public Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            return _collection.InsertManyAsync(entities);
        }

        public void Update(TEntity entity)
        {
            _collection.ReplaceOne(Filters.IdEq<TEntity>(entity.Id), entity);
        }

        public void Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> expression)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            return _collection.ReplaceOneAsync(Filters.IdEq<TEntity>(entity.Id), entity);
        }

        public Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> expression)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _collection.BulkWrite(CreateUpdates(entities));
        }

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            return _collection.BulkWriteAsync(CreateUpdates(entities));
        }

        private static IEnumerable<WriteModel<TEntity>> CreateUpdates(IEnumerable<TEntity> entities)
        {
            var updates = new List<WriteModel<TEntity>>();

            foreach (var entity in entities)
            {
                var id = entity.Id;

                if (id is null)
                {
                    continue;
                }

                updates.Add(new ReplaceOneModel<TEntity>(Filters.IdEq<TEntity>(id), entity));
            }

            return updates;
        }
    }
}
