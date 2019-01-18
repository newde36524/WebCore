using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebCore.Extension
{
    public class JWTHelper
    {
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }

        public static string EncodeToken(List<Claim> claims, string issuer, string audience, TimeSpan ExpDateTime)
        {
            var sign = new SigningCredentials(new RsaSecurityKey(GenerateKey()), SecurityAlgorithms.RsaSha256Signature);
            var header = new JwtHeader(sign);
            var payload = new JwtPayload(issuer, audience, claims, DateTime.Now, DateTime.Now + ExpDateTime);
            var security = new JwtSecurityToken(header, payload);
            var tokenHandle = new JwtSecurityTokenHandler();
            string token = tokenHandle.WriteToken(security);
            return token;
        }

        public static void DecodeToken(string token, Action<JwtSecurityToken> action)
        {
            var tokenHandle = new JwtSecurityToken(token);
            action(tokenHandle);
        }
    }
}
