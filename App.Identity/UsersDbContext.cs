using App.Identity.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Identity
{
    public class UsersDbContext : IdentityDbContext<Users>
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Organization>(org =>
            {
                org.ToTable("Organizations");
                org.HasKey(x => x.Id);

                org.HasMany<Users>()
                .WithOne(x => x.Organization)
                .HasForeignKey(x => x.OrgId)
                .IsRequired(false);
            }

            );
        }

    }
}

