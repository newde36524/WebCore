using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.WebApi
{
    [Route("api/Upload")]
    [ApiController]
    public class UploadController : Controller
    {
        [HttpPost(nameof(UploadFile))]
        public string UploadFile()
        {
            var file1 = this.Request.Form.Files["file001"];
            var file2 = this.Request.Form.Files["file002"];
            string msg = this.Request.Form["msg"];
            return $"文件1长度：{file1.Length}  文件2长度:{file2.Length}  msg:{msg}";
        }

        [HttpPost(nameof(UploadFile2))]
        public string UploadFile2([FromForm]IFormCollection form)
        {
            var file1 = form.Files["file001"];
            var file2 = form.Files["file002"];
            string msg = form["msg"];
            return $"文件1长度：{file1.Length}  文件2长度:{file2.Length}  msg:{msg}";
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
