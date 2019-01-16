using Microsoft.Extensions.Hosting;
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
        public MyRabbitMqService(IOptions<RabbitMqOption> options):base(options)
        {
        }

        public override bool Process(string message)
        {
            Console.WriteLine(message);
            return true;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(()=> Register(), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => base.UnRegister(), cancellationToken);
        }
    }
}
