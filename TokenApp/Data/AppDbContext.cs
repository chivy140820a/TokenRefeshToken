using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TokenApp.Configuration;
using TokenApp.Entity;

namespace TokenApp.Data
{
    public class AppDbContext : IdentityDbContext<AppUser,IdentityRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AppUserConfiguration());
            base.OnModelCreating(builder);
        }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
