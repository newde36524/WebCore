using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace WebCore.Fileters
{
    /// <summary>
    /// 只能捕获控制器级别的异常，在Filter中无效
    /// </summary>
    public class MyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            context.ExceptionHandled = true;//表示异常已被处理
            //context.Exception = null;//设置为null可以继续执行ResultFilter
            context.HttpContext.Response.Headers.Add("error", context.Exception.Message);//请求头参数不支持中文
            context.Result = new ContentResult { Content = "异常已处理" };
            //ViewResult
        }
    }
}
