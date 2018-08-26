using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebCore.Middleware
{
    public class AddEndpointMiddleware : BaseMiddleware
    {
        IHostingEnvironment _hostingEnvironment { get; set; }

        string temp { get; set; }
        public AddEndpointMiddleware(string sParam, RequestDelegate next, [FromServices]IHostingEnvironment hostingEnvironment) : base(next)
        {
            _hostingEnvironment = hostingEnvironment;
            temp = sParam;
        }

        public async override Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("remoteEndpoint", $"{context.Connection.RemoteIpAddress}:{context.Connection.RemotePort}");
            context.Response.Headers.Add("localEndpoint", $"{context.Connection.LocalIpAddress}:{context.Connection.LocalPort}");
            await base.Invoke(context);
        }
    }
}
