using DAL.Models;
using DTO;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TestChatR.Utils.Init
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            string adminLogin = "superAdmin";
            string AdminPassword = "_Aa123456";

            if (await roleManager.FindByNameAsync(Constants.ROLE_ADMIN) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Constants.ROLE_ADMIN));
            }
            if (await roleManager.FindByNameAsync(Constants.ROLE_USER) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Constants.ROLE_USER));
            }
            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                var admin = new User { UserName = adminLogin };
                IdentityResult result = await userManager.CreateAsync(admin, AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Constants.ROLE_ADMIN);
                }
            }
        }
    }
}
