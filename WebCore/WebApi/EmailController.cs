using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.WebApi
{
    [Route("api/EmailController")]
    public class EmailController : Controller
    {
        [HttpPost(nameof(Send_QQ))]
        public void Send_QQ(SendEmailRequest model)
        {
            Console.WriteLine($"通过QQ邮件接口向{model.Email}发送邮件，标题{model.Title}，内容：{model.Body}");
        }
        [HttpPost(nameof(Send_163))]
        public void Send_163(SendEmailRequest model)
        {
            Console.WriteLine($"通过网易邮件接口向{model.Email}发送邮件，标题{model.Title}，内容：{model.Body}");
        }
        [HttpPost(nameof(Send_Sohu))]
        public void Send_Sohu(SendEmailRequest model)
        {
            Console.WriteLine($"通过Sohu邮件接口向{model.Email}发送邮件，标题{model.Title}，内容：{model.Body}");
        }
    }
    public class SendEmailRequest
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
