using GymMangment.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Data.DbContexts
{
    public class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager
            , ILogger<IdentityDataSeeding> logger
            , CancellationToken ct = default)
        {
            try
            {
                var checkUser = await userManager.Users.AnyAsync();
                var checkRole = await roleManager.Roles.AnyAsync();
                if (checkRole && checkUser) return;
                var roles = new List<IdentityRole>
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin")
            };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed to create Role {role.Name} : {string.Join(" ; ", roleResult.Errors.Select(d => d.Description))}");
                        }
                        if (!checkUser)
                        {
                            var MainAdmin = new ApplicationUser
                            {
                                FirstName = "Antony",
                                LastName = "Girgis",
                                Email = "antonygirir@gmail.com",
                                UserName = "Antony Girgis",
                                PhoneNumber = "01264987530"
                            };
                            await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                            await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                            var Admin = new ApplicationUser
                            {
                                FirstName = "Andrew",
                                LastName = "Girgis",
                                Email = "andrewgirir@gmail.com",
                                UserName = "Antony Girgis",
                                PhoneNumber = "01264117530"
                            };
                            await userManager.CreateAsync(Admin, "P@ssw0rd");
                            await userManager.AddToRoleAsync(Admin, "Admin");
                            logger.LogInformation("Identity Data Seeded");
                            return;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Identity Seeded fail");
                return;
            }
        }
    }
}
