using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookStore_API.Data
{
    public static class SeedData
    {
        public static async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var seedRoles = SeedRoles(roleManager);
            var seedUsers = SeedUsers(userManager);
            await Task.WhenAll(seedRoles, seedUsers);
        }
        
        private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            var createAdminUser = CreateUser(userManager, "admin@bookstore.com", "Admin", "Administrator");
            var createCustomer1User = CreateUser(userManager, "customer@gmail.com", "JohnSmith", "Customer");
            var createCustomer2User = CreateUser(userManager, "customer2@gmail.com", "EricDupond", "Customer");
            await Task.WhenAll(createAdminUser, createCustomer1User, createCustomer2User);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var createAdministratorRole = CreateRole(roleManager, "Administrator");
            var createCustomerRole = CreateRole(roleManager, "Customer");
            await Task.WhenAll(createAdministratorRole, createCustomerRole);
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (! await roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole { Name = roleName };
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task CreateUser(UserManager<IdentityUser> userManager, string email, string username, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser { UserName = username, Email = email };
                var result = await userManager.CreateAsync(user, "P@ssw0rd!");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}