using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RiceManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RiceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ProjectBl5Context _context;
        private readonly AppSettings _appSettings;
        public AccountController(ProjectBl5Context context,IOptionsMonitor<AppSettings> optionsMonitor) {
            _context= context;
            _appSettings = optionsMonitor.CurrentValue;
        }
        [HttpPost]
        public IActionResult Login(UserViewModel userViewModel)
        {
            var user = _context.Users.SingleOrDefault(u=>u.Email== userViewModel.Email && u.Password == userViewModel.Password);
            if(user== null)
            {
                return Ok(new APIResponse
                {
                    Success = false,
                    Message = "Invalid"
                });
            }
            //get token
            return Ok(new APIResponse
            {
                Success = true,
                Message = "Successfull",
                Data = GenerateToken(user)
            });
        }

        private object GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var claims = new List<Claim>
        {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("user_id", user.UserId.ToString()),
        new Claim("TokenId", Guid.NewGuid().ToString())
         };

            if (user.RoleId != null)
            {
                // Truy vấn thông tin về vai trò dựa trên RoleId
                var role = GetRoleFromDatabase(user.RoleId.Value);
                if (role != null)
                {
                    // Thêm thông tin về vai trò vào token
                    claims.Add(new Claim(ClaimTypes.Role, role.Name.Trim()));
                }
            }

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
        private Role GetRoleFromDatabase(int roleId)
        {
            return _context.Roles.FirstOrDefault(r => r.RoleId == roleId);
        }
    }

}
