using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyWebApiApp.Data;
using MyWebApiApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Validate(LoginModel model)
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
                var token = await GenderateToken(user);
                return Ok(new APIResponse
                {
                    Success = true,
                    Message = "authenticate sucess",
                    Data = token
                });
            }
        }
        private async Task<TokenModel> GenderateToken(User user)
        {
            var jwtTokenhandle = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {

                    new Claim(ClaimTypes.Name, user.Name),

                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Id", user.Id.ToString()),

                    new Claim(ClaimTypes.Role, user.Role) /// this is (roles = "")

                    //roles
                  

                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenhandle.CreateToken(tokenDescription);
            var accessToken = jwtTokenhandle.WriteToken(token);
            var refeshToken = GenerateRefreshToken();

            //save to database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.Id,
                Token = refeshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),
            };
            await _context.AddAsync(refreshTokenEntity);
            _context.SaveChangesAsync();
            return new TokenModel
            {
                AccessToken = accessToken,
                RefeshToken = refeshToken
            };

        }
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //supply token
                ValidateIssuer = false,
                ValidateAudience = false,
                // sign on token

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false // uncheck token expire time
            };
            try
            {
                // check 1:AcessToken valid format
                var tokenInverification = jwtTokenHandler.ValidateToken(model.AccessToken,
                                                                        tokenValidateParam, out var validatedToken);
                // check 2 : check alig
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals
                        (SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return Ok(new APIResponse
                        {
                            Success = false,
                            Message = "invalid"
                        });
                    }
                }

                // check 3: Access token expire?
                var UtcExpireDate = long.Parse(tokenInverification.Claims.FirstOrDefault(
                                    x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(UtcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "token not expire"
                    });
                }

                // check 4: refresh token in DB
                var storedToken = _context.RefeshTokens.FirstOrDefault(x => x.Token == model.RefeshToken);
                if (storedToken == null)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }


                // check 5: check refeshtoken Used/ revoked
                if (storedToken.IsUsed)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "refesh token has been used"
                    });
                }
                if (storedToken.IsUsed)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "refesh token has been revoked"
                    });
                }

                //check 6 : access id == jwtId in refeshToken
                var jti = tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "token doesn't match"
                    });
                }
                // Upddate Token is Used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token 
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == storedToken.UserId);
                var token = await GenderateToken(user);

                return Ok(new APIResponse
                {
                    Success = true,
                    Message = "success"

                });

            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = "error"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}

