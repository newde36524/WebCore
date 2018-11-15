using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Filters;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Aspect
{
    /// <summary>
    /// 重试
    /// </summary>
    public class RetryAttribute : Attribute, IAsyncActionFilter
    {
        public RetryAttribute(int retryCount)
        {
            RetryCount = retryCount;
        }

        public int RetryCount { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
            //await Policy
            //    .Handle<Exception>()
            //    .Retry(RetryCount)
            //    .ExecuteAsync(async () => await next());
        }
    }
}
