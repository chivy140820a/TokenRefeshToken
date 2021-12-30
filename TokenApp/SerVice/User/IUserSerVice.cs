using TokenApp.Entity;
using TokenApp.ViewModel;

namespace TokenApp.SerVice.User
{
    public interface IUserSerVice
    {
        Task<AppUser> FindUserByName(string UserName);
        Task<TokenResponse> Authentication(AuthenticationRequest request);
        Task<bool> Register(RegisterRequest request);
    }
}
