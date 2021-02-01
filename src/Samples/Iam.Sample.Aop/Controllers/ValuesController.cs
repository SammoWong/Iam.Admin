using Iam.Core.Attributes;
using Iam.Core.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iam.Sample.Aop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICachingService _cachingService;

        public ValuesController(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }

        [HttpGet]
        [ElapsedTimer]
        public DateTime GetCurrentTime()
        {
            return _cachingService.GetCurrentTime();
        }

        [HttpGet]
        public long GetCurrentTimestamp() => _cachingService.GetCurrentTimestamp(DateTime.Now);

        [HttpGet]
        public Data GetData() => _cachingService.GetData();

        [HttpGet]
        public async Task<Data> GetDatasAsync()
        {
            return await _cachingService.GetDataAsync();
        }


    }
}
