using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Extension.Options;

namespace WebCore.Extension
{
    /// <summary>
    /// RabbitListenerAbstract 额外定义一个抽象父类，是为了满足不是所有子类都需要满足 IHostedService 接口应用场景的需求
    /// </summary>
    public abstract class RabbitListenerAbstract
    {
        public RabbitListenerAbstract(IOptions<RabbitMqOption> options)
        {
            Options = options;
        }

        public IModel Channel { get; set; }
        public IConnection Connection { get; set; }

        public IOptions<RabbitMqOption> Options { get; }

        /// <summary>
        /// 处理消息的方法
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract bool Process(string message);

        /// <summary>
        /// 注册消费者监听
        /// </summary>
        protected void Register()
        {
            var factory = new ConnectionFactory()
            {
                HostName = Options.Value.RabbitHost,
                UserName = Options.Value.RabbitUserName,
                Password = Options.Value.RabbitPassword,
                Port = Options.Value.RabbitPort
            };
            this.Connection = factory.CreateConnection();
            this.Channel = Connection.CreateModel();
            Console.WriteLine($"RabbitListener register,routeKey:{Options.Value.RouteKey}");
            Channel.ExchangeDeclare(exchange: Options.Value.Extrange, type: "topic");
            Channel.QueueDeclare(queue: Options.Value.QueueName, exclusive: false);
            Channel.QueueBind(queue: Options.Value.QueueName,
                              exchange: Options.Value.Extrange,
                              routingKey: Options.Value.RouteKey);
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var result = Process(message);
                if (result)
                {
                    Channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            Channel.BasicConsume(queue: Options.Value.QueueName, consumer: consumer);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        protected void UnRegister()
        {
            this.Connection.Close();
        }
    }
}
