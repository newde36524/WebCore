using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WebCore.Fileters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.WebApi
{
    [Route("api/Download")]
    [ApiController]
    public class DownloadController : Controller
    {
        public static IHostingEnvironment _hostingEnvironment { get; set; }

        public DownloadController([FromServices]IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 获取图片流
        /// </summary>
        /// <returns></returns>
        [MyActionFilter]
        //[DoorChainFilter(@"https://localhost:44320/test", _hostingEnvironment)]
        [TypeFilter(typeof(DoorChainFilterAttribute))]
        [HttpGet(nameof(GetImg))]
        public IActionResult GetImg()
        {
            var path = Path.Combine($"{_hostingEnvironment.ContentRootPath}", "StaticSource", "Images", "a.jpg");
            var data = System.IO.File.ReadAllBytes(path);
            //this.Response.Body.Write(data, 0, data.Length);//也可以直接指定body,但不能和File一起使用
            return File(System.IO.File.OpenRead(path), "image/jpg");
        }

        [HttpGet(nameof(GetImg2))]
        public IActionResult GetImg2()
        {
            var path = Path.Combine($"{_hostingEnvironment.ContentRootPath}", "StaticSource", "Images", "a.jpg");
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                {
                    { ".apk","application/vnd.android.package-archive"}
                });//自动检测文件contenttype,也可以自定义添加文件映射类型
            if (fileExtensionContentTypeProvider.TryGetContentType(path, out string contentype))
            {
                return this.File(System.IO.File.OpenRead(path), contentype);
            }
            return NotFound();
        }

        [HttpGet("GetImg/{fileName}")]
        public IActionResult GetImg(string fileName)
        {
            var path = Path.Combine($"{_hostingEnvironment.ContentRootPath}", "StaticSource", "Images", fileName);
            if (System.IO.File.Exists(path))
            {
                var data = System.IO.File.ReadAllBytes(path);
                //this.Response.Body.Write(data, 0, data.Length);//也可以直接指定body,但不能和File一起使用
                return File(System.IO.File.OpenRead(path), "image/jpg");
            }
            else
            {
                path = Path.Combine($"{_hostingEnvironment.ContentRootPath}", "StaticSource", "Images", "404.jpg");
                return File(System.IO.File.OpenRead(path), "image/jpg");
            }
        }

        /// <summary>
        /// 提供图片下载
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(DownLoadFile))]
        public IActionResult DownLoadFile()
        {
            var path = Path.Combine($"{_hostingEnvironment.ContentRootPath}", "StaticSource", "Images", "a.jpg");
            return File(System.IO.File.OpenRead(path), "application/octet-stream", "a.jpg");
        }

        /// <summary>
        /// 获取IHostingEnvironment的注入实现类并返回
        /// </summary>
        /// <returns></returns>
        [MyActionFilter]
        [HttpGet(nameof(GetHost))]
        public IActionResult GetHost()
        {
            Span<byte> span = new byte[100];
            Span<int> span2 = new List<int>().ToArray();

            RandomNumberGenerator.Fill(span);

            using (HttpClient httpClient = new HttpClient(new SocketsHttpHandler()))
            {

            }


            return this.Json(_hostingEnvironment);
        }
    }
}
