using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebCore.Middleware
{
    public class SampleMiddleware : BaseMiddleware
    {
        public SampleMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) : base(next)
        {
            LoggerFactory = loggerFactory;
        }

        public ILoggerFactory LoggerFactory { get; }

        public async override Task Invoke(HttpContext context)
        {
            //befor
            LoggerFactory.AddConsole();
            context.Response.Headers.Add("SampleMiddlewareBefore", "SampleMiddlewareBefore");
            await base.Invoke(context);
            //after
            context.Response.Headers.Add("SampleMiddlewareAfter", "SampleMiddlewareAfter");
        }
    }
}
