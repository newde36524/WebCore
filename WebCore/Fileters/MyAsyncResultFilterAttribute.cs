using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    public class MyAsyncResultFilterAttribute : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            Console.WriteLine("返回请求结果前");
            await next();
            Console.WriteLine("返回请求结果后");
        }
    }
}
