using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    public class MyActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("在具体事件管道阶段之后调用");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //context.ActionArguments//通过属性可修改传入的参数

            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Console.WriteLine("在具体事件管道阶段之前调用");
            //context.Result = new ContentResult
            //{
            //    Content = "Action拦截"
            //};
        }
    }
}
