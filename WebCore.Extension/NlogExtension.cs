using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace WebCore.Extension
{
    public static class NlogExtension
    {
        /// <summary>
        /// 启用Nlog日志扩展方法，日志配置参数查看nlog.config文件
        /// </summary>
        /// <param name="webHostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseNLoger(this IWebHostBuilder webHostBuilder) =>
            AspNetExtensions.UseNLog(webHostBuilder.ConfigureLogging((webHostBuilderContext, logging) =>
            {
                logging.AddFilter("System", LogLevel.Warning);//不记录系统自带的异常日志
                logging.AddFilter("Microsoft", LogLevel.Warning);//不记录微软自带的异常日志
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            }));
    }
}
