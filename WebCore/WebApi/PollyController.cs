using Microsoft.AspNetCore.Mvc;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Aspect;

namespace WebCore.WebApi
{
    [ApiController]
    [Route("TestPolly")]
    public class PollyController : Controller
    {
        [Retry(50)]
        [HttpPost(nameof(TestRetry))]
        public IActionResult TestRetry()
        {
            //Policy.Timeout(300).ExecuteAsync(ct=> { },c);

            return this.Redirect(nameof(Redirect));
        }


        [HttpGet(nameof(Redirect))]
        public string Redirect()
        {
            return "跳转请求";
        }

    }
}
