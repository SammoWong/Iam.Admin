using Iam.EventBus.Abstractions;
using Iam.Sample.EventBus.EventHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iam.Sample.EventBus.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public MessageController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        /// <summary>
        /// 测试EventBus😀
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public string EventBus(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _eventBus.Publish(new MessageReceivedIntegrationEvent { Message = message });
            }
            return "OK";
        }
    }
}
