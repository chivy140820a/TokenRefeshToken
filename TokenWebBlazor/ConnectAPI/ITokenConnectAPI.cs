using System.Security.Claims;
using TokenApp.ViewModel;

namespace TokenWebBlazor.ConnectAPI;

public interface ITokenConnectAPI
{
    void RefeshToken(TokenApiModel request);
    Task<ClaimsPrincipal> GetClaimsPrincipal(string token);

}
