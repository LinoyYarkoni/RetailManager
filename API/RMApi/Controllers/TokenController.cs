using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RMApi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RMApi.Controllers
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string grant_type)
        {
            if(await isValidCredentials(username, password))
            {
                return new ObjectResult(await generateToken(username));
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> isValidCredentials(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);

            return await _userManager.CheckPasswordAsync(user, password);
        }

        private async Task<dynamic> generateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var roles = from u in _context.UserRoles
                        join r in _context.Roles
                        on u.RoleId equals r.Id
                        where u.UserId == user.Id
                        select new { u.UserId, u.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(
                    JwtRegisteredClaimNames.Nbf, 
                    new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(
                    JwtRegisteredClaimNames.Exp,
                    new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken
            (
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsVeryVerySecretAndVeryVeryLong")),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims)
            );

            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = username
            };

            return output;
        }
    }
}
