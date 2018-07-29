using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    public class ShortCircuitingResourceFilterAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
           
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //一旦设置了Result属性，请求管道直接返回
            context.Result = new ContentResult()
            {
                Content = "资源被拦截，所有过滤器失效，Action失效"
            };
        }
    }
}
