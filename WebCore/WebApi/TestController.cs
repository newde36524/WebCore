using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.CustomerActionResult;
using WebCore.Fileters;

namespace WebCore.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet(nameof(GetResult))]
        public string GetResult()
        {
            return "测试Get请求";
        }

        [HttpPost(nameof(PostResult))]
        public string PostResult()
        {
            return "测试Post请求";
        }

        [HttpGet(nameof(GetReultWithParam))]
        public string GetReultWithParam(string msg, string msg2)
        {
            return $"get {msg} {msg2}";
        }

        [HttpPost(nameof(PostResultWithParam))]
        public string PostResultWithParam([FromForm]string msg, [FromForm]string msg2)
        {
            return $"post {msg} {msg2}";
        }

        [HttpPost(nameof(PostResultWithModel))]
        public MyClass[] PostResultWithModel([FromBody]MyClass model)
        {
            return new[] { model };
        }

        //[HttpPost(nameof(UploadFile))]
        //public string UploadFile([FromForm]IFormCollection form)
        //{
        //    var file1 = form.Files["file001"];
        //    var file2 = form.Files["file002"];
        //    string msg = form["msg"];
        //    return $"文件1长度：{file1.Length}  文件2长度:{file2.Length}  msg:{msg} 111";
        //}
        [CustomerResultFilter]
        [HttpGet(nameof(TestMyContentResult))]
        public IActionResult TestMyContentResult()
        {
            //return new ContentResult()
            //{
            //    Content = JsonConvert.SerializeObject(new
            //    {
            //        msg = "123",
            //        msg2 = "456"
            //    })
            //};

            return new MyContentResult()
            {
                Content = new
                {
                    msg = "123",
                    msg2 = "456"
                }
            };
        }
    }

    public class MyClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
