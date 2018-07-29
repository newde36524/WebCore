using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    /// <summary>
    /// 只能捕获控制器级别的异常，在Filter中无效
    /// </summary>
    public class MyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //context.HttpContext.Response.Headers.Add("error", context.Exception.Message);
            base.OnException(context);
        }
    }
}
