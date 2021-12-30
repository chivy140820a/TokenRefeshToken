using Microsoft.AspNetCore.Identity;
using TokenApp.Data;
using TokenApp.Entity;
using TokenApp.SerVice.Token;
using TokenApp.ViewModel;

namespace TokenApp.SerVice.User
{
    public class UserSerVice : IUserSerVice
    {
        private readonly ITokenSerVice _tokenSerVice;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public UserSerVice(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config
            , ITokenSerVice tokenSerVice )
        {

            _tokenSerVice = tokenSerVice;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<TokenResponse> Authentication(AuthenticationRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                throw new Exception("Tk không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claimUser = new ClaimUserLogin()
            {
                UserName = request.UserName,
                Email = request.UserName,
                Roles = roles
            };
            var token = _tokenSerVice.GenerateAccessToken(claimUser);
            var refeshToken = _tokenSerVice.GenerateRefreshToken();
            user.RefeshToken = refeshToken;
            user.StartTime = DateTime.Now;
            await _context.SaveChangesAsync();
            user.ExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            return new TokenResponse()
            {
                AccessToken = token,
                RefeshToken = refeshToken
            };
        }

        public async Task<AppUser> FindUserByName(string UserName)
        {
            var find = await _userManager.FindByNameAsync(UserName);
            return find;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };
            var create = await _userManager.CreateAsync(user, request.Password);
            if (create.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
