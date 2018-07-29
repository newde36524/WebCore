using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCore.Fileters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.WebApi
{
    [MyResultFilter]
    [MyActionFilter]
    [MyFormatFilter]
    [MyAuthorizationFilter]
    //[MyServiceFilter(typeof(FileterController))]//添加后无法访问了？？？
    //[MyTypeFilter(typeof(FileterController))]
    [Route("api/Fileter")]
    [ApiController]
    public class FileterController : Controller
    {
        //[MyResourceFilter]
        [ShortCircuitingResourceFilter]
        [HttpGet(nameof(ShortCircuitingResource))]
        public string ShortCircuitingResource()
        {
            return "如果成功被拦截，消息不返回，而是拦截过滤器返回的信息";
        }

        [HttpGet(nameof(GetFileterExcutedMsg))]
        public IActionResult GetFileterExcutedMsg()
        {
            return Content("经过各种过滤器之后的返回信息，观察http网络请求");
        }

        [MyExceptionFilter]
        [HttpGet(nameof(TestExceptionFileter))]
        public IActionResult TestExceptionFileter()
        {
            throw new Exception("throw Exception");
        }

    }
}
