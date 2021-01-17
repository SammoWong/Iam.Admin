using Iam.Core;
using Iam.Core.ApiModels;
using Iam.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Iam.AspNetCore.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CustomException exception)
            {
                context.Result = new JsonResult(new ApiResult
                {
                    Code = exception.Code,
                    Message = context.Exception.Message
                });
            }
            else
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                {
                    context.Result = new JsonResult(new ApiResult<Exception>
                    {
                        Code = ErrorCode.Default,
                        Message = context.Exception.Message,
                        Data = context.Exception
                        //description = context.Exception.Message,
                        //stackTrace = context.Exception.StackTrace
                    });
                }
                else
                {
                    context.Result = new JsonResult(new ApiResult<Exception>
                    {
                        Code = ErrorCode.Default,
                        Message = "System Error",
                        Data = context.Exception
                        //description = context.Exception.Message,
                        //stackTrace = context.Exception.StackTrace
                    });
                }
            }
        }
    }
}
