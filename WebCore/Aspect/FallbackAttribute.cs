using Microsoft.AspNetCore.Mvc.Filters;
using Polly;
using System;
using System.Threading.Tasks;

namespace WebCore.Aspect
{
    /// <summary>
    /// 回滚
    /// </summary>
    public class FallbackAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await Policy.Handle<Exception>().FallbackAsync(cancle =>
            {
                //触发异常时可执行

                return Task.CompletedTask;
            }).ExecuteAsync(() => next());
        }
    }
}
