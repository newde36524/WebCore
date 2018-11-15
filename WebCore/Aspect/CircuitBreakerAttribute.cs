using Microsoft.AspNetCore.Mvc.Filters;
using Polly;
using System;
using System.Threading.Tasks;

namespace WebCore.Aspect
{
    /// <summary>
    /// 熔断
    /// </summary>
    public class CircuitBreakerAttribute : Attribute, IAsyncActionFilter
    {
        public CircuitBreakerAttribute() { }
        /// <summary>
        /// 熔断器特性的构造函数
        /// </summary>
        /// <param name="exceptionsAllowedBeforeBreaking">异常触发次数</param>
        /// <param name="durationOfBreak">恢复间隔</param>
        public CircuitBreakerAttribute(int exceptionsAllowedBeforeBreaking, int durationOfBreak)
        {
            ExceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            DurationOfBreak = TimeSpan.FromMilliseconds(durationOfBreak);
        }

        public int ExceptionsAllowedBeforeBreaking { get; set; }

        public TimeSpan DurationOfBreak { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await Policy.Handle<Exception>().CircuitBreakerAsync(ExceptionsAllowedBeforeBreaking, DurationOfBreak).ExecuteAsync(() => next());
        }
    }
}
