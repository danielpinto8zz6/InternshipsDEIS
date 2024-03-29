using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InternshipsDEIS.Models;

namespace InternshipsDEIS
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider services)
        {
            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();

            await CreateRolesAsync(roleManager);

            var userManager = services
                .GetRequiredService<UserManager<ApplicationUser>>();
            await EnsureTestAdminAsync(userManager);
        }

        private static async Task CreateRolesAsync(
    RoleManager<IdentityRole> roleManager)
        {
            string[] RoleNames = { "Administrator", "Company", "Student", "Professor", "Committee" };

            foreach (var Role in RoleNames)
            {
                var alreadyExists = await roleManager
                    .RoleExistsAsync(Role);

                if (alreadyExists) return;

                await roleManager.CreateAsync(
                    new IdentityRole(Role));
            }
        }

        private static async Task EnsureTestAdminAsync(
    UserManager<ApplicationUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync();

            if (testAdmin != null) return;

            testAdmin = new ApplicationUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };
            await userManager.CreateAsync(
                testAdmin, "NotSecure123!!");
            await userManager.AddToRoleAsync(
                testAdmin, "Administrator");
        }
    }
}