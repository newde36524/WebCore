using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCore.Extension.Options;

namespace WebCore.Hosting
{
    public class MyRabbitMqDeclareClient : IQueuePublish
    {
        public IModel Channel { get; set; }
        public IOptions<RabbitMqOption> Options { get; }
        public ILogger<MyRabbitMqService> Logger { get; }

        public MyRabbitMqDeclareClient(IOptions<RabbitMqOption> options, ILogger<MyRabbitMqService> logger, IModel Channel)
        {
            Options = options;
            Logger = logger;
            this.Channel = Channel;
        }

        public void Publish(object message)
        {
            Channel.QueueDeclare(queue: Options.Value.QueueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: true,
                                        arguments: null);
            string msgJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(msgJson);
            Channel.BasicPublish(exchange: Options.Value.Extrange,
                                    routingKey: Options.Value.RouteKey,
                                    basicProperties: null,
                                    body: body);
        }
    }
}
