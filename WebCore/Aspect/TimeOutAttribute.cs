using Microsoft.AspNetCore.Mvc.Filters;
using Polly;
using System;
using System.Threading.Tasks;

namespace WebCore.Aspect
{
    /// <summary>
    /// 超时
    /// </summary>
    public class TimeOutAttribute : Attribute, IAsyncActionFilter
    {
        public TimeOutAttribute(TimeSpan timeout)
        {
            Timeout = timeout;
        }

        public TimeSpan Timeout { get; set; }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await Policy.TimeoutAsync(Timeout).ExecuteAsync(() => next());
        }
    }
}
