using Iam.Data.Entities;
using Iam.Data.MongoDB.Context;
using Iam.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.MongoDB.Repositories
{
    public class MongoRepository<TEntity> : Repository<TEntity> where TEntity : MongoEntity
    {
        public MongoRepository(IMongoContext context) : base(new MongoCommandRepository<TEntity>(context), new MongoQueryRepository<TEntity>(context)) { }
    }
}
