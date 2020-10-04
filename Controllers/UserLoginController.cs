using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserAuthentication.Models;
using UserAuthentication.Settings;

namespace UserAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : ControllerBase
    {
        List<UserLoginModel> userModels;
        private readonly AppSettings _appSettings;
        public UserLoginController(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
            userModels = new List<UserLoginModel>
            {
                new UserLoginModel
                {
                    UserName="Sijo",
                    Password="1234",
                    Role="Admin"
            },
                new UserLoginModel
                {
                    UserName="TestUser",
                    Password="1234",
                    Role="User"
                },
            };
        }

        [HttpGet]
        public IEnumerable<UserLoginModel> Get()
        {
            return this.userModels;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody]UserLoginModel login)
        {
            IActionResult response = Unauthorized();
            UserLoginModel user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJWTToken(user);
                user.Token = tokenString;
                response = Ok(user);
            }
            return response;
        }
        UserLoginModel AuthenticateUser(UserLoginModel loginModel)
        {
            var user = userModels.Where(s => s.UserName == loginModel.UserName && s.Password == loginModel.Password).SingleOrDefault();

            return user;
        }
        string GenerateJWTToken(UserLoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),

            new Claim("role",userInfo.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
            issuer: _appSettings.Issuer,
            audience: _appSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }


}