using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Core.ApiModels
{
    public class ApiResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

    }

    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }
}
