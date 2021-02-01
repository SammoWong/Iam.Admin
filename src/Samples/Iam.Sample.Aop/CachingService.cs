using Iam.Core.Aop;
using Iam.Core.Attributes;
using Iam.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iam.Sample.Aop
{
    public interface ICachingService
    {
        DateTime GetCurrentTime();

        long GetCurrentTimestamp(DateTime? dateTime);

        Data GetData();

        Task<Data> GetDataAsync();
    }

    public class CachingService : ICachingService, IAopService
    {
        [Caching(nameof(GetCurrentTime), 2 * 60)]
        [ElapsedTimer]
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        [Caching(nameof(GetCurrentTimestamp), 2 * 60)]
        [ElapsedTimer]
        public long GetCurrentTimestamp(DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToUniversalTime().Ticks : DateTime.Now.ToUniversalTime().Ticks;
        }

        [Caching(nameof(GetData), 2 * 60)]
        public Data GetData()
        {
            return new Data();
        }

        [Caching(nameof(GetDataAsync), 2 * 60)]
        public async Task<Data> GetDataAsync()
        {
            return await Task.FromResult(new Data());
        }
    }

    public class Data
    {
        public Data()
        {
            Guid = Guid.NewGuid();
            Today = DateTime.Today;
        }

        public Guid Guid { get; set; }

        public DateTime Today { get; set; }  
    }
}
