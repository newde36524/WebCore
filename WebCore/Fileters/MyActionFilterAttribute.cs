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
            //设置所有域名都可以请求，允许跨域访问
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //context.ActionArguments//通过属性可修改传入的参数
            Console.WriteLine("在具体事件管道阶段之前调用");
            //context.Result = new ContentResult
            //{
            //    Content = "Action拦截"
            //};
        }
    }
}
