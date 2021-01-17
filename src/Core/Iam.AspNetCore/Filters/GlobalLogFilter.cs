using Iam.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Iam.AspNetCore.Filters
{
    public class GlobalLogFilter : IAsyncActionFilter
    {
        private readonly ILogger<GlobalLogFilter> _logger;

        public GlobalLogFilter(ILogger<GlobalLogFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string requestId = context.HttpContext.TraceIdentifier;
            var watch = new Stopwatch();
            watch.Start();
            ActionExecutedContext result = await next();
            watch.Stop();
            if (result.Exception == null)
            {
                LogInformation(context, result, watch, requestId);
            }
            else
            {
                LogError(context, watch, result.Exception, requestId);
            }
        }

        /// <summary>
        /// 记录访问日志
        /// </summary>
        /// <param name="executingContext"></param>
        /// <param name="watch"></param>
        /// <param name="requestId"></param>
        /// <param name="executedContext"></param>
        private void LogInformation(ActionExecutingContext executingContext, ActionExecutedContext executedContext, Stopwatch watch, string requestId)
        {
            object response_content = null;
            if (executedContext.Result is ObjectResult objectResult)
                response_content = objectResult.Value;

            var log = new LogModel()
            {
                requestId = requestId,
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ms"),
                elapsed_time = watch.Elapsed.TotalMilliseconds,
                interface_name = executingContext.ActionDescriptor.DisplayName,
                request_content = executingContext.ActionArguments,
                response_content = response_content,
                source_ip = executingContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                status = executedContext.HttpContext.Response.StatusCode,
                request_header = JsonConvert.SerializeObject(executingContext.HttpContext.Request.Headers),
                current_userid = executedContext.HttpContext.User.Identity.IsAuthenticated ?
                (int.TryParse(executingContext.HttpContext?.User.Claims?.First(u => u.Type == ClaimTypes.NameIdentifier)?.Value, out int currentUserId) ? currentUserId : 0)
                : 0
            };
            _logger.LogInformation(JsonConvert.SerializeObject(log));
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="watch"></param>
        /// <param name="exception"></param>
        /// <param name="requestId"></param>
        private void LogError(ActionExecutingContext context, Stopwatch watch, Exception exception, string requestId)
        {
            object response_content = null;
            var log = new LogModel()
            {
                requestId = requestId,
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ms"),
                elapsed_time = watch.Elapsed.TotalMilliseconds,
                interface_name = context.ActionDescriptor.DisplayName,
                request_content = context.ActionArguments,
                response_content = response_content,
                source_ip = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                status = exception is CustomException ex ? ex.Code : (int)HttpStatusCode.InternalServerError,
                msg = exception.ToString(),
                request_header = JsonConvert.SerializeObject(context.HttpContext.Request.Headers),
                current_userid = context.HttpContext.User.Identity.IsAuthenticated ?
                (int.TryParse(context.HttpContext?.User.Claims?.First(u => u.Type == ClaimTypes.NameIdentifier)?.Value, out int currentUserId) ? currentUserId : 0)
                : 0
            };
            if (exception is CustomException customException)
            {
                log.msg = $"code:{customException.Code},msg:{customException.Message}";
                _logger.LogInformation(JsonConvert.SerializeObject(log));
            }
            else
            {
                _logger.LogError(JsonConvert.SerializeObject(log));
            }
        }
    }
    internal class LogModel
    {
        /// <summary>
        /// 日志产生时间
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 接口调用返回码，返回码意义参照全局错误代码
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 接口调用耗时，ms为单位
        /// </summary>
        public double elapsed_time { get; set; }

        /// <summary>
        /// 调用方服务名称
        /// </summary>
        public string source_srv { get; set; }

        /// <summary>
        /// 调用方来源ip
        /// </summary>
        public string source_ip { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string interface_name { get; set; }

        /// <summary>
        /// 请求体内容，可以细化为更详细的字段
        /// </summary>
        public object request_content { get; set; }

        /// <summary>
        /// 响应体内容，可以细化为更详细的字段
        /// </summary>
        public object response_content { get; set; }

        /// <summary>
        ///  消息体，此字段作为扩展字段，可以为空，也可以存放发生异常时一些错误堆栈信息之类的
        /// </summary>
        public string msg { get; set; }

        public string request_header { get; set; }

        public int current_userid { get; set; }

        public string requestId { get; set; }
    }
}
