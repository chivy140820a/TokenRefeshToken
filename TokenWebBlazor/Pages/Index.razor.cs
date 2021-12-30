using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using TokenApp.ViewModel;
using TokenWebBlazor.ConnectAPI;

namespace TokenWebBlazor.Pages
{
    public partial class Index
    {
        [Inject]
        public ILocalStorageService _localStorage { get; set; }
        [Inject]
        public ITokenConnectAPI _tokenConnectAPI { get; set; }
        protected  override async Task OnInitializedAsync()
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
    }
}
