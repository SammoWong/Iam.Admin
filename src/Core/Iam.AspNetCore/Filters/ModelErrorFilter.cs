using Iam.Core;
using Iam.Core.ApiModels;
using Iam.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.AspNetCore.Filters
{
    public class ModelErrorFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new JsonResult(new ApiResult 
                { 
                    Code = ErrorCode.Default, 
                    Message = context.ModelState.AllModelStateErrors().FirstOrDefault()?.Message//获取第一个错误信息
                });
            }
            else
            {
                await next();
            }
        }
    }
}
