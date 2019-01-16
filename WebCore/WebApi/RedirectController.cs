using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebCore.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        /// <summary>
        /// 测试重定向Action
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult RedirectAction(string url)
        {
            return Redirect(url);

            if (Url.IsLocalUrl(url))//指示参数是否拥有当前域名地址
            {
                return Redirect(url);
            }
            else
            {
                return (new ActionResult<bool>(false) as IConvertToActionResult).Convert();
            }
        }

        [HttpGet("[action]")]
        public IActionResult RedirectAction1(string url)
        {
            return RedirectPermanent(url);
        }

        [HttpGet("[action]")]
        public IActionResult RedirectAction2(string url)
        {
            return RedirectPreserveMethod(url);
        }
    }
}