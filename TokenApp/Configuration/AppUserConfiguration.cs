using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TokenApp.Entity;

namespace TokenApp.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.RefeshToken);
            builder.Property(x => x.ExpiryTime);
            builder.Property(x => x.StartTime);
        }
    }
}
