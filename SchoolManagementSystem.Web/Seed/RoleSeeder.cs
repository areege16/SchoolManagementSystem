using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Application.Account;
using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.Web.Seed
{
    public class RoleSeeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var roles = Roles.AllowedRoles;

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }
        }
    }
}
