using Iam.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iam.AspNetCore.Filters
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if(controllerActionDescriptor == null)
            {
                await next();
                return;
            }

            var unitOfWorkAttr = controllerActionDescriptor.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            if(unitOfWorkAttr != null)
            {
                _unitOfWork.BeginTransaction();
            }

            var result = await next();

            if (result.Exception == null || result.ExceptionHandled)
            {
                _unitOfWork.Commit();
            }
        }
    }
}
