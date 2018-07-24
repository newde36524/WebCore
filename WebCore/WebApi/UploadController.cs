using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.WebApi
{
    [Route("api/UploadController")]
    public class UploadController : Controller
    {
        [HttpPost(nameof(UploadFile))]
        public void UploadFile([FromBody]byte[] file)
        {

        }
        [HttpGet(nameof(Hehe))]
        public string Hehe()
        {
            return "hehe";
        }
        //[HttpPost("Upload2")]
        //public void Upload([FromBody]Stream file)
        //{

        //}
    }
}
