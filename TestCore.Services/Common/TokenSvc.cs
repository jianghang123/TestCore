using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestCore.Common;
using TestCore.Domain.ApiEntity;
using TestCore.Domain.Entity;
using TestCore.IService.Common;

namespace TestCore.Services.Common
{
    public class TokenSvc : ITokenSvc
    {
        public AuthTokenDTO CreateToken(Users member)
        {

            var token = string.Empty;

            var claims = new Claim[] {
                 new Claim(ClaimTypes.Name,member.Id.ToString()),
                 new Claim(ClaimTypes.Sid,member.Username.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebConfig.JWTSettings.Secret));//WebConfig.AppSettings.SingleSignIn ? member.JWTSecret : WebConfig.JWTSettings.Secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var securityToken = new JwtSecurityToken(
                                 issuer: WebConfig.JWTSettings.Issuer,
                                 audience: WebConfig.JWTSettings.Audience,
                                 claims: claims,
                                 notBefore: now,
                                 expires: now.AddMinutes(WebConfig.JWTSettings.AccessExpiration),
                                 signingCredentials: signingCredentials
                                 );

            token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new AuthTokenDTO { AccessToken = token, ExpireInMinutes = WebConfig.JWTSettings.AccessExpiration, MemId = member.Id };
        }
    }
}
