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
        public static async Task SeedIdentityDataAsync(
     RoleManager<IdentityRole> roleManager,
     UserManager<ApplicationUser> userManager,
     ILogger<IdentityDataSeeding> logger,
     CancellationToken ct = default)
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
                            logger.LogError($"Failed to create Role {role.Name} : {string.Join(" ; ", roleResult.Errors.Select(d => d.Description))}");
                    }
                }

              
                if (!checkUser)
                {
                    var mainAdmin = new ApplicationUser
                    {
                        FirstName = "Antony",
                        LastName = "Girgis",
                        Email = "antonygirir@gmail.com",
                        UserName = "AntonyGirgis",  
                        PhoneNumber = "01264987530"
                    };
                    var mainResult = await userManager.CreateAsync(mainAdmin, "P@ssw0rd");
                    if (mainResult.Succeeded)
                        await userManager.AddToRoleAsync(mainAdmin, "SuperAdmin");
                    else
                        logger.LogError($"Failed to create MainAdmin: {string.Join(" ; ", mainResult.Errors.Select(e => e.Description))}");

                    var admin = new ApplicationUser
                    {
                        FirstName = "Andrew",
                        LastName = "Girgis",
                        Email = "andrewgirir@gmail.com",
                        UserName = "AndrewGirgis",    
                        PhoneNumber = "01264117530"
                    };
                    var adminResult = await userManager.CreateAsync(admin, "P@ssw0rd");
                    if (adminResult.Succeeded)
                        await userManager.AddToRoleAsync(admin, "Admin");
                    else
                        logger.LogError($"Failed to create Admin: {string.Join(" ; ", adminResult.Errors.Select(e => e.Description))}");

                    logger.LogInformation("Identity Data Seeded Successfully");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity Seeding Failed");
            }
        }
    }
}
