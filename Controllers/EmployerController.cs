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
    public class EmployerController : ControllerBase
    {
        private readonly EmployerService _employerService;
        private readonly IConfiguration _config;

        public EmployerController(EmployerService employerService, IConfiguration config)
        {
            _employerService = employerService;
            _config = config;
        }

        [Route("getProfile")]
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "employer"
        )]
        public async Task<Employer> GetEmployers()
        {
            var curr_user = GetCurrentUser();
            return await _employerService.Getprofile(curr_user.name);
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> register([FromBody] Employer employerInfo)
        {
            var result = await _employerService.RegisterEmployer(employerInfo);
            if (result)
            {
                return Ok("Registration successful");
            }
            return BadRequest("registration failed");
        }

        [HttpPut("edit")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "employer"
        )]
        public async Task<IActionResult> editUSerInfo([FromBody] EmployerUpdate employerInfo)
        {
            var curr_user = GetCurrentUser();
            var result = await _employerService.UpdateEmployer(curr_user.name, employerInfo);
            if (result)
            {
                return Ok("updated successfully");
            }
            return BadRequest("update failed");
        }

        [HttpDelete("delete/{id}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "employer"
        )]
        public async Task<IActionResult> DeleteByID(string id)
        {
            var result = _employerService.DeleteEmployersync(id);
            if (result)
            {
                return Ok("employer deleted successfully");
            }
            return BadRequest("deleting failed");
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
            var employer = await _employerService.EmployerLogin(loginInfo);
            if (employer == null)
            {
                return BadRequest("No account found");
            }
            var token = GenerateToken(employer.Id, employer.role);
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
