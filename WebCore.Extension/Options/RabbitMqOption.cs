using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Extension.Options
{
    public class RabbitMqOption : IOptions<RabbitMqOption>
    {
        public RabbitMqOption Value => this;

        public string RabbitHost { get; set; }
        public string RabbitUserName { get; set; }
        public string RabbitPassword { get; set; }
        public int RabbitPort { get; set; }
        public string RouteKey { get; set; }
        public string QueueName { get; set; }
        public string Extrange { get; set; }
    }
}
