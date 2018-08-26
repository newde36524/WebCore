using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    public class AjaxRequestFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (IsAjaxRequest(context.HttpContext.Request))
            {

            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("hehe", "hello");
            //var data = Encoding.UTF8.GetBytes("hello");
            //context.HttpContext.Response.Body.Write(data, 0, data.Length);
            if (IsAjaxRequest(context.HttpContext.Request))
            {
                context.Result = new ContentResult() { Content = "不允许ajax请求" };
            }
        }

        public bool IsAjaxRequest(HttpRequest httpRequest)
        {
            return httpRequest.Headers["X-Requested-With"].Equals("XMLHttpRequest");
        }
    }
}
