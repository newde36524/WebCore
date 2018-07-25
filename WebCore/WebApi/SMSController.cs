using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCore.WebApi
{
    [Route("api/SMS")]
    [ApiController]
    public class SMSController : Controller
    {
        [HttpPost(nameof(Send_MI))]
        public dynamic Send_MI([FromBody]dynamic model)
        {
            Console.WriteLine($"通过小米短信接口向{model.phoneNum}发送短信{model.Msg}");
            return model;
        }

        [HttpPost(nameof(Send_LX))]
        public SendSMSRequest Send_LX([FromBody]SendSMSRequest model)
        {
            Console.WriteLine($"通过联想短信接口向{model.PhoneNum}发送短信{model.Msg}");
            this.HttpContext.Response.Cookies.Append("测试coockie","helloworld");
            var cookie = this.HttpContext.Request.Cookies["测试coockie"];
            model.Cookie = cookie;
            return model;
        }

        [HttpPost(nameof(Send_HW))]
        public void Send_HW([FromBody]SendSMSRequest model)
        {
            Console.WriteLine($"通过华为短信接口向{model.PhoneNum}发送短信{model.Msg}");
        }

    }

    public class SendSMSRequest
    {
        public int Id { get; set; }
        public string PhoneNum { get; set; }
        public string Msg { get; set; }
        public string Cookie { get; set; }
    }
}
