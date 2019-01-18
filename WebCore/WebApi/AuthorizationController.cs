using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using WebCore.Extension;

namespace WebCore.WebApi
{
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }

        [HttpGet("[action]/{username}/{password}/{verifyCode}")]
        public string GetToken(string username, string password, string verifyCode)
        {
            var result = JWTHelper.EncodeToken(new List<Claim> {
                new Claim ("password",username),
                //new Claim ("password",password),//没必要把密码也发布出去，token发布后，只在乎exp生命周期
                new Claim ("verifyCode",verifyCode),
            }, "WebCore", "customer", TimeSpan.FromHours(2));
            return result;
        }

        [HttpGet("[action]/{token}")]
        public object DecToken(string token)
        {
            JwtSecurityToken result = null;
            JWTHelper.DecodeToken(token, sec =>
            {
                result = sec;
            });
            return result.Payload;
        }
    }
}
