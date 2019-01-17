using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebCore.Extension;
using WebCore.Extension.Options;

namespace WebCore.Hosting
{
    public class MyRabbitMqService : RabbitListenerAbstract, IHostedService
    {
        public MyRabbitMqService(IOptions<RabbitMqOption> options, ILogger<MyRabbitMqService> logger) : base(options)
        {
            Logger = logger;
        }

        public ILogger<MyRabbitMqService> Logger { get; }

        public override bool Process(string message)
        {
            Logger.LogInformation(message);
            return true;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    Register();
                    Logger.LogInformation("RabbitMq开始监听");
                }
                catch (Exception)
                {
                    Logger.LogWarning("RabbitMq监听失败");
                }
            }, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    base.UnRegister();
                    Logger.LogInformation("RabbitMq取消监听");
                }
                catch (Exception)
                {
                    Logger.LogInformation("RabbitMq取消监听时，发生异常");
                }
            }, cancellationToken);
        }
    }
}
