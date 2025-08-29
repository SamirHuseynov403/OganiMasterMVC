using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Data.DataContext
{
    public class DataInit
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public DataInit(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedData()
        {
            _context.Database.Migrate();
            await CreateSuperAdmin();
        }
        public async Task CreateSuperAdmin()
        {
            List<string> roles = new List<string> { "SuperAdmin", "Admin", "Moderator", "User" };
            foreach (var role in roles)
            {
                var hasRole = await _roleManager.RoleExistsAsync(role);
                if (!hasRole)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
            var existUser = await _userManager.FindByNameAsync("superadmin");

            if (existUser != null) return;

            var superAdmin = new AppUser
            {
                UserName = "superAdmin",
                Email = "superadmin@"
            };
            var result = await _userManager.CreateAsync(superAdmin, "1111");

            if (!result.Succeeded) return;

            await _userManager.AddToRolesAsync(superAdmin,roles);
        }
    }
}
