using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebCore.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HttpWebApiHelperMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpWebApiHelperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //todo  定义一个路由 表示需要获取Api列表 从整个程序集中 反射找到标注了[ApiController]特性的类


            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpWebApiHelperMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpWebApiHelperMiddleware(this IApplicationBuilder builder, IEnumerable<Controller> controllerList)
        {
           
            // 使用当前中间件，就定一个路由规则，
            // 给传进来的控制器列表反射成Api列表 
            // 包含请求方式 请求模板 请求路径 请求参数

            //var result = new List<object>();

            //var result = controllerList.Select(controller =>
            //{
            //    var type = controller.GetType();
            //    var methods = type.GetMethods().Where(method =>//拿到标注了访问特性的Action
            //    {
            //        return method.GetCustomAttributes(true).Where(attr => attr.GetType().BaseType == typeof(HttpMethodAttribute)).Count() > 0;
            //    });





            //    return null;
            //});


            /**
             [
                {
                    controllerName:"TestApi",
                    actionList:[
                        {
                            method:"GET",
                            template:"api/TestApi/Show/{msg}",
                            url:"localhost:12345/api/TestApi/Show/{msg}",
                            params:[""]
                        },
                    ]
                },
            ]
             
             
             
             
             
             */


            return builder.UseMiddleware<HttpWebApiHelperMiddleware>();
        }
    }
}
