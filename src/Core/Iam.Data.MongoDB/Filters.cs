using Iam.Data.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.MongoDB
{
    public static class Filters
    {
        public static FilterDefinition<TEntity> IdEq<TEntity>(string id) where TEntity : MongoEntity
        {
            return Builders<TEntity>.Filter.Eq(x => x.Id, id);
        }
    }
}
