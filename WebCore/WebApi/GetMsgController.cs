using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.WebApi
{
    [ApiController]
    [Route("api/GetMsg")]
    public class GetMsgController : Controller
    {
        [HttpGet(nameof(GetProductList))]
        public IEnumerable<Product> GetProductList()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "T430笔记本", Price = 8888, Description = "CPU i7标压版，1T硬盘" },
                new Product { Id = 2, Name = "华为Mate10", Price = 3888, Description = "大猩猩屏幕，多点触摸" },
                new Product { Id = 3, Name = "天梭手表", Price = 9888, Description = "瑞士经典款，可好了" }
            };
        }
        [HttpGet("GetModel/{id:int}/{name}/{price:int}/{description}")]
        public Product GetModel([FromRoute]int id, [FromRoute]string name, [FromRoute]int price, [FromRoute]string description)
        {
            return new Product { Id = id, Name = name, Price = price, Description = description };
        }
        [HttpGet("GetModel")]
        public Product GetModel([FromQuery] Product product)
        {
            return product;
        }

        [HttpGet("GetModel2")]
        public Product GetModel2([FromHeader] Product product)
        {
            return product;
        }


        [HttpPost(nameof(Send_MI))]
        public SendSMSRequest Send_MI([FromBody]SendSMSRequest model) => model;

        [HttpPost(nameof(Send_LX))]
        public SendSMSRequest Send_LX([FromBody]SendSMSRequest model)
        {
            Console.WriteLine($"通过联想短信接口向{model.PhoneNum}发送短信{model.Msg}");
            var cookie = this.HttpContext.Request.Cookies["test_coockie"];
            this.HttpContext.Response.Cookies.Append("test_coockie", "helloworld");
            model.Cookie = cookie;
            return model;
        }

        [HttpGet(nameof(GetFormData))]
        public IFormCollection GetFormData([FromServices]IHostingEnvironment hostingEnvironment)
        {
            FormFileCollection formFiles = new FormFileCollection();
            var path = Path.Combine($"{hostingEnvironment.ContentRootPath}", "StaticSource", "Images", "a.jpg");
            var stream = System.IO.File.OpenRead(path);
            var formFile = new FormFile(stream, 0, stream.Length, "myFile", "test.jpg");
            //formFile.ContentType = "image/jpg";//bug  设置会抛异常
            formFiles.Add(formFile);//无效

            FormCollection form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()
            {
                { "key1","测试1"},
                { "key2","测试2"},
                { "key3","测试3"},
                { "key4","测试4"}
            }, formFiles);
            return form;
        }

    }

    public class SendSMSRequest
    {
        public int Id { get; set; }
        public string PhoneNum { get; set; }
        public string Msg { get; set; }
        public string Cookie { get; set; }
    }
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
