namespace TokenApp.ViewModel
{
    public class ClaimUserLogin
    {
        public string UserName { get;set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        
    }
}
