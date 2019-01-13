using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestNlogController : Controller
    {
        public TestNlogController(ILogger<TestNlogController> logger)
        {
            Logger = logger;
        }

        public ILogger<TestNlogController> Logger { get; }

        [HttpGet("[action]/{msg}")]
        public string Log(string msg)
        {
            Logger.LogInformation(msg);
            return msg;
        }
    }
}
