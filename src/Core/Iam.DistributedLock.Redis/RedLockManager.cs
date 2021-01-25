using Iam.Core.DistributedLock;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
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
        private readonly RedLockFactory _redlockFactory;
        private readonly List<IRedLock> ManagedLocks = new List<IRedLock>();

        public RedLockManager(string connectionString)
        {
            _redlockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> { ConnectionMultiplexer.Connect(connectionString) });
        }

        public async Task<bool> AcquireLock(string id, TimeSpan expiryTime, CancellationToken? cancellationToken = null)
        {
            var redLock = await _redlockFactory.CreateLockAsync(id, expiryTime);
            if (redLock.IsAcquired)
            {
                lock (ManagedLocks)
                {
                    ManagedLocks.Add(redLock);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> AcquireLock(string id, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            var redLock = await _redlockFactory.CreateLockAsync(id, expiryTime, waitTime, retryTime);

            if (redLock.IsAcquired)
            {
                lock (ManagedLocks)
                {
                    ManagedLocks.Add(redLock);
                }
                return true;
            }
            return false;
        }

        public Task ReleaseLock(string id)
        {
            lock (ManagedLocks)
            {
                foreach (var redLock in ManagedLocks)
                {
                    if (redLock.Resource == id)
                    {
                        redLock.Dispose();
                        ManagedLocks.Remove(redLock);
                        break;
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
