using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Data;
using MyWebApiApp.Models;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSetting _appSetting;
        public UserController(MyDbContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _appSetting = optionsMonitor.CurrentValue;
        }
        [HttpPost("Login")]
        public IActionResult Validate(LoginModel model)
        {
            {
                var user = _context.Users.SingleOrDefault(u => u.UserName == model.UserName
                && model.Password == u.Password);
                if (user == null)
                {
                    {
                        return Ok(new APIResponse
                        {
                            Success = false,
                            Message = "invalid user name or password"
                        });
                    }
                }
                // supply token 
                return Ok(new APIResponse
                {
                    Success = true,
                    Message = "authenticate sucess",
                    Data = GenderateToken(user)
                });
            }
        }
        private string GenderateToken(User user)
        {
            var jwtTokenhandle = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {

                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserName", user.UserName),
                    new Claim("Id", user.Id.ToString()),

                    //roles
                    new Claim("TokenId", Guid.NewGuid().ToString())

                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenhandle.CreateToken(tokenDescription);
            return jwtTokenhandle.WriteToken(token);
        }
    }
}

