

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TokenApp.ViewModel;

namespace TokenWebBlazor.ConnectAPI;

public class UserConnectAPI : IUserConnectAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public UserConnectAPI(HttpClient httpClient, ILocalStorageService localStorageService
        , AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorageService;
        _httpClient = httpClient;
    }

    public async Task<string> Authentication(AuthenticationRequest request)
    {
        var find = await _httpClient.PostAsJsonAsync("/api/User/Authentication", request);
        var read = await find.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<TokenResponse>(read);
        await _localStorage.SetItemAsync("ResToken", token.RefeshToken);
        await _localStorage.SetItemAsync("authToken", token.AccessToken);
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(request.UserName);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        return "/";
    }
}
