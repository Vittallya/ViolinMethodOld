using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.EF_Core
{
    public class DbMainContext: IdentityDbContext<IdentityUser>
    {
        public DbMainContext(DbContextOptions<DbMainContext> opt):base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "8429384",
                Name = "admin",
                NormalizedName = "ADMIN"

            });

            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "9034928",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "admin"),
                SecurityStamp = string.Empty
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "8429384",
                UserId = "9034928"
            });
        }
    }
}
