using Microsoft.AspNetCore.Identity;
using AuraPerfumes.Constants;
using AuraPerfumes.Models;
using AuraPerfumes.Data;
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            var context = service.GetRequiredService<ApplicationDbContext>();
            //adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));
            //create admin user
            if (!context.Genders.Any())
            {
                context.Genders.AddRange(
                    new Gender { GenderLabel = "Male" },
                    new Gender { GenderLabel = "Female" },
                    new Gender { GenderLabel = "Unisex" }
                );

                await context.SaveChangesAsync();
            }
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }

        }
    }
}
