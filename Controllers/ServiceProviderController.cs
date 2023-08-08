using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using web.Models;
using web.Services;

namespace web.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ServiceProviderController : ControllerBase
    {
        private readonly ServiceProviderService _ServiceProviderService;
        private readonly IConfiguration _config;

        public ServiceProviderController(
            ServiceProviderService ServiceProviderService,
            IConfiguration config
        )
        {
            _ServiceProviderService = ServiceProviderService;
            _config = config;
        }

        [Route("getProfile")]
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var curr_user = GetCurrentUser();

            var profile = _ServiceProviderService.GetProfile(curr_user.name);
            if (profile == null)
            {
                return BadRequest("no information was found");
            }
            return Ok(profile);
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> register([FromBody] UserInformation UserInformation)
        {
            var result = await _ServiceProviderService.CreateAsync(UserInformation);
            if (result)
            {
                return Ok("Registration successfull");
            }
            return BadRequest("Wrong input");
        }

        [HttpPut("edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public IActionResult editUSerInfo([FromBody] UpdateModel updateInfo)
        {
            var curr_user = GetCurrentUser();

            var result = _ServiceProviderService.UpdateUsers(curr_user.name, updateInfo);
            if (result)
            {
                return Ok("updated successully");
            }
            return BadRequest("update failed");
        }

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public IActionResult DeleteByID()
        {
            var curr_user = GetCurrentUser();
            var deleted = _ServiceProviderService.Delete(curr_user.name);
            if (deleted)
            {
                return Ok("deleted successfully");
            }
            return NotFound("user not found");
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> LogIn(LoginModel loginInfo)
        {
            if (
                string.IsNullOrWhiteSpace(loginInfo.name)
                || string.IsNullOrWhiteSpace(loginInfo.password)
            )
            {
                return BadRequest("invalid input");
            }
            var usr = await _ServiceProviderService.ServiceProviderLogin(loginInfo);
            if (usr == null)
            {
                return BadRequest("No user found");
            }
            var token = GenerateToken(usr.Id, usr.role);
            return Ok(token);
        }

        private string GenerateToken(string id, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role)
            };
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private LoginModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new LoginModel
                {
                    name = userClaims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                        ?.Value,
                    password = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
