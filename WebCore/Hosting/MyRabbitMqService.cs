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
        public MyRabbitMqService(IOptions<RabbitMqOption> options, ILogger<MyRabbitMqService> logger) :base(options)
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
            await Task.Run(()=> Register(), cancellationToken);
            Logger.LogInformation("RabbitMq开始监听");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => base.UnRegister(), cancellationToken);
            Logger.LogInformation("RabbitMq取消监听");
        }
    }
}
