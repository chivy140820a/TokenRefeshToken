namespace TokenApp.ViewModel
{
    public class AuthenticationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
