using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenApp.SerVice.User;
using TokenApp.ViewModel;

namespace TokenApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IUserSerVice _userSerVice;
        public UserController(IUserSerVice userSerVice)
        {
            _userSerVice = userSerVice;
        }
        [HttpPost("Authentication")]
        public async Task<IActionResult> Authen([FromBody] AuthenticationRequest request)
        {
            var auth = await _userSerVice.Authentication(request);
            return Ok(auth);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var find = await _userSerVice.Register(request);
            return Ok();
        }
    }
}
