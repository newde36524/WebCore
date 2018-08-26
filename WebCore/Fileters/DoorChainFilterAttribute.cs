using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Fileters
{
    /// <summary>
    /// 图片防盗链过滤器
    /// 不要添加默认无参构造函数，通过TypeFilterAttribute 使用自定义过滤器特性时，会默认使用无参构造函数
    /// </summary>
    public class DoorChainFilterAttribute : Attribute, IActionFilter
    {
        string _imgRequestReferer { get; set; } = "https://localhost:44320";
        IHostingEnvironment _hostingEnvironment { get; set; }

        public DoorChainFilterAttribute([FromServices]IHostingEnvironment hostingEnvironment)
        {
            //_imgRequestReferer = imgRequestReferer;
            _hostingEnvironment = hostingEnvironment as IHostingEnvironment;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(context.HttpContext.Request.Headers["Referer"]))
            {
                //Headers 获取制定头信息的值时，务必调用ToString方法获取，否则直接使用时，在正确界面也会进入条件分支而显示404图片
                if (!context.HttpContext.Request.Headers["Referer"].ToString().Contains(_imgRequestReferer))
                {
                    var path = Path.Combine($"{_hostingEnvironment.ContentRootPath}", "StaticSource", "Images", "404.jpg");
                    context.Result = new FileStreamResult(File.OpenRead(path), "image/jpg");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //migration
            //do nothing
        }
    }
}
