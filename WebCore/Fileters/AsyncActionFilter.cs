using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    /// <summary>
    /// 当一个过滤器同时实现IAsyncActionFilter和IActionFilter接口时，优先执行IAsyncActionFilter
    /// </summary>
    public class AsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("在具体事件管道阶段之前调用");
            await next();//具体处理的管道
            Console.WriteLine("在具体事件管道阶段之后调用");
        }
    }
}
