using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {

        //username and password
        [HttpPost("Authunticate")]
        public IActionResult Authunticate([FromBody]AuthunicationModel authunication)
        {
            if (string.IsNullOrEmpty(authunication.Username) || string.IsNullOrEmpty(authunication.Password))
                return Unauthorized();
            if (authunication.Username == authunication.Password)
            {
                //Here Authentcation is success
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("BattulaNaveen9640@");
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, authunication.Username),
                        new Claim(ClaimTypes.Role,"Admin")
                    }),
                    Expires=DateTime.UtcNow.AddDays(7),
                    SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)

                };
            var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwttoken =tokenHandler.WriteToken(token);
                return Ok(jwttoken);
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
           var claims = User.Claims.ToList();
           var name = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            return Ok(new List<string>() {"Naveen","Reavanth","Rajivan"});
        }
    }
}
