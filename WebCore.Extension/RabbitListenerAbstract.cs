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

        private IConnection connection;
        private IModel channel;

        protected string RouteKey { get; set; }
        protected string QueueName { get; set; }
        protected string Exchange { get; set; } = "message";
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
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Options.Value.RabbitHost,
                    UserName = Options.Value.RabbitUserName,
                    Password = Options.Value.RabbitPassword,
                    Port = Options.Value.RabbitPort
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
                Console.WriteLine($"RabbitListener register,routeKey:{RouteKey}");
                channel.ExchangeDeclare(exchange: Exchange, type: "topic");
                channel.QueueDeclare(queue: QueueName, exclusive: false);
                channel.QueueBind(queue: QueueName,
                                  exchange: Exchange,
                                  routingKey: RouteKey);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var result = Process(message);
                    if (result)
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                };
                channel.BasicConsume(queue: QueueName, consumer: consumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        protected void UnRegister()
        {
            this.connection.Close();
        }
    }
}
