using Iam.Core.DistributedLock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iam.DistributedLock.Redis
{
    public class RedLockManager : IDistributedLockManager
    {
        public Task<bool> AcquireLock(string Id, TimeSpan expiryTime, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AcquireLock(string Id, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task ReleaseLock(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
