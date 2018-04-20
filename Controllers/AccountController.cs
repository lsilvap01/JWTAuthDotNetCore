using JWTAuthentication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [Authorize]
        [HttpGet("me")]
        public IActionResult Home()
        {
            return Ok(new {
                first_name = "Admin",
                last_name = "istrador",
                friends = new string[] { "friend1", "friend2" }
            });
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult RequestToken([FromBody] TokenRequest request, [FromServices] IOptions<TokenConfiguration> tokenConfigurationOption)
        {
            if(request.Username != "admin" || request.Password != "1234")
                return BadRequest("Could not verify username and password");

            var tokenConfiguration = tokenConfigurationOption.Value;
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username)
            };

            var key = tokenConfiguration.GetSymmetricSecurityKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                                issuer: tokenConfiguration.Issuer,
                                audience: tokenConfiguration.Audience,
                                claims: claims,
                                expires: DateTime.Now.AddSeconds(tokenConfiguration.ExpirationInSeconds),
                                signingCredentials: creds
                            );
            
            return Ok(new {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}