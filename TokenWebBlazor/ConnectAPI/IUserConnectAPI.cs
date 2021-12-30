using TokenApp.ViewModel;

namespace TokenWebBlazor.ConnectAPI;

public interface IUserConnectAPI
{
    Task<string> Authentication(AuthenticationRequest request);
}
