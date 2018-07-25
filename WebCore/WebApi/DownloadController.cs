using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.WebApi
{
    [Route("api/Download")]
    [ApiController]
    public class DownloadController : Controller
    {
        [HttpGet(nameof(DowloadFile))]
        public byte[] DowloadFile()
        {
            var result = new byte[] { 0x12};
            this.Response.ContentType = "image/png";
            return result;
        }

    }
}
