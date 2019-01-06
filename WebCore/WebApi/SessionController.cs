using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCore.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : Controller
    {

        [HttpPost("[action]")]
        public IActionResult TestIntoSession([FromForm]string key, [FromForm]string value)
        {
            var session = this.HttpContext.Session;
            if (!session.Keys.Contains(key))
            {
                this.RedirectToAction("TestIntoSessionRed", "Session", new { key, value });
            }
            return Ok();
        }
        public IActionResult TestIntoSessionRed()
        {
            return Ok();
        }
        public IActionResult TestIntoSessionRed([FromForm]string key, [FromForm]string value)
        {
            var session = this.HttpContext.Session;
            session.Set(key, Encoding.UTF8.GetBytes(value));
            return Ok();
        }

        [HttpGet("[action]/{key}")]
        public IActionResult TestGetSessionValue(string key)
        {
            var session = this.HttpContext.Session;
            return Content(session.TryGetValue(key, out byte[] v) ? Encoding.UTF8.GetString(v) : string.Empty);
        }

    }
}
