using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    public class MyAsyncExceptionFilterAttribute : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
