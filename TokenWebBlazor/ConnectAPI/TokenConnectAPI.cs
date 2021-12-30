using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using TokenApp.ViewModel;

namespace TokenWebBlazor.ConnectAPI;

public class TokenConnectAPI : ITokenConnectAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public TokenConnectAPI(HttpClient httpClient, ILocalStorageService localStorageService
        , AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorageService;
        _httpClient = httpClient;
    }

    public async Task<ClaimsPrincipal> GetClaimsPrincipal(string token)
    {
        var find = await _httpClient.PostAsJsonAsync("/api/Token/GetClaimsPrincipal", token);
        var read = await find.Content.ReadAsStringAsync();
        var res = JsonConvert.DeserializeObject<ClaimsPrincipal>(read);
        return res;
    }



    public async void RefeshToken(TokenApiModel request)
    {
        var find = await _httpClient.PostAsJsonAsync("/api/Token/Refresh", request);
        var read = await find.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<TokenResponse>(read);
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("ResToken");
        await _localStorage.SetItemAsync("authToken", token.AccessToken);
        await _localStorage.SetItemAsync("ResToken", token.RefeshToken);
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(token.UserName);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
    }
}
