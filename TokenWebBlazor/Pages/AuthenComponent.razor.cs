using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using TokenApp.SerVice.User;
using TokenApp.ViewModel;
using TokenWebBlazor.ConnectAPI;

namespace TokenWebBlazor.Pages
{
    public partial class AuthenComponent
    {
        [Inject]
        public ILocalStorageService _localStorage { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        [Inject]
        public IUserConnectAPI _userConnectAPI { get; set; }
        [Inject]
        public ITokenConnectAPI _tokenConnectAPI { get; set; }
        public AuthenticationRequest authenticationRequest { get; set; } = new AuthenticationRequest();
        protected override async Task OnInitializedAsync()
        {
            var ass = await _localStorage.GetItemAsync<string>("authToken");
            var res = await _localStorage.GetItemAsync<string>("ResToken");
            var request = new TokenApiModel()
            {
                AccessToken = ass,
                RefreshToken = res
            };
            _tokenConnectAPI.RefeshToken(request);
        }
        public async Task OnSubmit()
        {
            var url = await _userConnectAPI.Authentication(authenticationRequest);
            navigationManager.NavigateTo("/");
        }
    }
}
