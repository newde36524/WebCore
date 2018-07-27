using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    public class ActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("在具体事件管道阶段之后调用");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("在具体事件管道阶段之前调用");
        }
    }
}
