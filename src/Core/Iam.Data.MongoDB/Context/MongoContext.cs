using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.MongoDB.Context
{
    public abstract class MongoContext : IMongoContext
    {
        protected MongoContext(string connectionString)
        {
            Database = new MongoClient(connectionString).GetDatabase(new MongoUrl(connectionString).DatabaseName);
        }

        public IMongoDatabase Database { get; }

        IMongoDatabase IMongoContext.Database => throw new NotImplementedException();
    }                                           
}
