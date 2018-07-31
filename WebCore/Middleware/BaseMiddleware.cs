using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Middleware
{
    public abstract class BaseMiddleware
    {
        public BaseMiddleware(RequestDelegate next)
        {
            Next = next;
        }
      
        /// <summary>
        /// 执行下一个中间件的委托，需要使用构造注入
        /// </summary>
        private RequestDelegate Next { get; set; }

        /// <summary>
        /// 约定俗成 自定义中间件需要一个Invoke方法，使用时 框架自动注入
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async virtual Task Invoke(HttpContext context)
        {
            await Next(context);
        }
    }
}
