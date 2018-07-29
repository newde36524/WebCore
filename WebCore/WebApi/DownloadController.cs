using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
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
        public IHostingEnvironment _hostingEnvironment { get; set; }

        public DownloadController([FromServices]IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 获取图片流
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetImg))]
        public IActionResult GetImg()
        {
            var path = Path.Combine($"{_hostingEnvironment.ContentRootPath}", "StaticSource", "Images", "a.jpg");
            var file = System.IO.File.ReadAllBytes(path);
            return new FileContentResult(file, "image/jpg");
        }

        [HttpGet(nameof(GetImg2))]
        public IActionResult GetImg2()
        {
            var path = Path.Combine($"{_hostingEnvironment.ContentRootPath}", "StaticSource", "Images", "a.jpg");
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                {
                    { ".apk","application/vnd.android.package-archive"}
                });//自动检测文件contenttype,也可以自定义添加文件映射类型
            var file = System.IO.File.ReadAllBytes(path);
            if (fileExtensionContentTypeProvider.TryGetContentType(path, out string contentype))
            {
                return this.File(file, contentype);
            }
            return NotFound();
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
        public IHostingEnvironment GetHost()
        {
            return _hostingEnvironment;
        }
    }
}
