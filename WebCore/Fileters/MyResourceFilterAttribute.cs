using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    public class MyResourceFilterAttribute : Attribute, IResourceFilter
    {
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("资源请求后");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("资源请求前");
            context.Result = new ContentResult
            {
                Content = "资源请求前适合做缓存"
            };
        }
    }
}
