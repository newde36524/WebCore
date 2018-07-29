using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    public class AddHeaderResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);

            throw new Exception("测试异常过滤器在Fileter中是否也有效");//因为已经返回，所以请求已经成功，所以这里抛异常无效
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
            context.HttpContext.Response.Headers.Add("ResultFilter", "OnResultExecuting");
            //throw new Exception("测试异常过滤器在Fileter中是否也有效");//正常返回500
        }
    }
}
