using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using WebCore.Hosting;

namespace WebCore.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestRabbitMqController : Controller
    {
        public TestRabbitMqController(ILogger<TestNlogController> logger, IQueuePublish queuePublish)
        {
            Logger = logger;
            QueuePublish = queuePublish;
        }

        public ILogger<TestNlogController> Logger { get; }
        public IQueuePublish QueuePublish { get; }

        [HttpGet("[action]/{msg}")]
        public string Publish(string msg)
        {
            QueuePublish.Publish(msg);
            return msg;
        }
    }
}
