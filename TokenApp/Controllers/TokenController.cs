using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TokenApp.Data;
using TokenApp.Entity;
using TokenApp.SerVice.Token;
using TokenApp.SerVice.User;
using TokenApp.ViewModel;

namespace TokenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserSerVice _userSerVice;
        private readonly AppDbContext _context;

        private readonly ITokenSerVice _tokenSerVice;
        public TokenController(ITokenSerVice tokenSerVice, AppDbContext context, IUserSerVice userSerVice
            , UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _userSerVice = userSerVice;
            _context = context;
            _tokenSerVice = tokenSerVice;
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _tokenSerVice.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
           
            var user = await _userSerVice.FindUserByName(username);
            var roles = await _userManager.GetRolesAsync(user);
            if (user == null || user.RefeshToken != refreshToken || user.ExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var utcExpireDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
            var dateNow = DateTime.Now;
            if (expireDate > dateNow)
            {
                return BadRequest("Invalid client request");
            }
            var request = new ClaimUserLogin()
            {
                UserName = user.UserName,
                Email = user.UserName,
                Roles  = roles
            };
            var newAccessToken = _tokenSerVice.GenerateAccessToken(request);
            var newRefreshToken = _tokenSerVice.GenerateRefreshToken();
            user.RefeshToken = newRefreshToken;
            _context.SaveChanges();
            var res = new TokenResponse()
            {
                UserName = username,
                AccessToken = newAccessToken,
                RefeshToken = newRefreshToken
            };
            return Ok(res);
        }
        [HttpPost("GetClaimsPrincipal")]
        public IActionResult GetClaimsPrincipal([FromBody]string token)
        {
            var claim = _tokenSerVice.GetPrincipalFromExpiredToken(token);
            return Ok(claim);
        }
        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(utcExpireDate).ToLocalTime();
            return dateTime;
        }
        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var user = await _userSerVice.FindUserByName(username);
            if (user == null) return BadRequest();
            user.RefeshToken = null;
            await _userManager.UpdateAsync(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
