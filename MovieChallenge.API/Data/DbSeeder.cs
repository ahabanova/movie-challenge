using Microsoft.AspNetCore.Identity;
using MovieChallenge.API.Models;

namespace MovieChallenge.API.Data
{
    public static class DbSeeder
    {
        public async static Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<AppUser> user, IConfiguration configuration)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (await user.FindByEmailAsync(configuration["AdminUser:Email"]!) == null)
            {
                var newUser = new AppUser
                {
                    UserName = configuration["AdminUser:Email"],
                    Email = configuration["AdminUser:Email"],
                    Name = configuration["AdminUser:Name"]!
                };
                await user.CreateAsync(newUser, configuration["AdminUser:Password"]!);
                await user.AddToRoleAsync(newUser, "Admin");
            }
        }
    }
}