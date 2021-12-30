using Microsoft.AspNetCore.Identity;

namespace TokenApp.Entity
{
    public class AppUser:IdentityUser
    {
        public string? RefeshToken { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? ExpiryTime { get; set; }
    }
}
