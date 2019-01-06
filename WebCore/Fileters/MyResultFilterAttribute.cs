using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebCore.CustomerActionResult;

namespace WebCore.Fileters
{
    public class MyResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
            Console.WriteLine("返回请求结果后");
            throw new Exception("测试异常过滤器在Fileter中是否也有效");//因为已经返回，所以请求已经成功，如果不配置ResourceFilter这里抛异常无效,否则请求返回500
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //这里是最后一道过滤管道关口，之后的OnResultExecuted 执行对这次请求响应无效，但可以继续做一些后续处理
            base.OnResultExecuting(context);
            Console.WriteLine("返回请求结果前");
            context.HttpContext.Response.Headers.Add("ResultFilter", "OnResultExecuting");
            //throw new Exception("测试异常过滤器在Fileter中是否也有效");//正常返回500
        }
    }

    public class CustomerResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
            var result = context.Result;
            context.Result = new MyContentResult
            {
                Content = new
                {
                    code = 0,
                    result
                }
            };
        }
    }

}
