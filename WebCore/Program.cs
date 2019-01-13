﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace WebCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            //BuildWebHost(args).Run();//根据配置文件监听
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()//启用Nlog日志
                .UseStartup<Startup>()
                //.UseUrls("http://127.0.0.1:5002")
            ;

        public static IWebHost BuildWebHost(string[] args) {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            string ip = config["ip"];
            string port = config["port"];
            Console.WriteLine($"ip={ip},port={port}");
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.ConfigureLogging(logging =>
                //{
                //    logging.ClearProviders();
                //    logging.SetMinimumLevel(LogLevel.Trace);
                //})
                //.UseNLog()
                .UseUrls($"http://{ip}:{port}")
                .Build();
        }

    }
}
