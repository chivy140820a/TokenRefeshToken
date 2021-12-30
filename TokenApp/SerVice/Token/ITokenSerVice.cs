using System.Security.Claims;
using TokenApp.ViewModel;

namespace TokenApp.SerVice.Token
{
    public interface ITokenSerVice
    {
        string GenerateAccessToken(ClaimUserLogin request);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
