using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using stagesDEIS.Models;

namespace stagesDEIS
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider services)
        {
            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();

            await CreateRoleAsync(roleManager, Constants.AdministratorRole);
            await CreateRoleAsync(roleManager, Constants.StudentRole);
            await CreateRoleAsync(roleManager, Constants.ProfessorRole);
            await CreateRoleAsync(roleManager, Constants.CompanyRole);

            var userManager = services
                .GetRequiredService<UserManager<ApplicationUser>>();
            await EnsureTestAdminAsync(userManager);
        }

        private static async Task CreateRoleAsync(
    RoleManager<IdentityRole> roleManager, String role)
        {
            var alreadyExists = await roleManager
                .RoleExistsAsync(role);

            if (alreadyExists) return;

            await roleManager.CreateAsync(
                new IdentityRole(role));
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
                testAdmin, Constants.AdministratorRole);
        }
    }
}