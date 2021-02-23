using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Data.MongoDB.Context
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }
}
