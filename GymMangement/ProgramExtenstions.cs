using GymMangment.DAL.Data.DataSeeding;
using GymMangment.DAL.Data.DbContexts;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace GymMangement.PL
{
    public static class ProgramExtenstions
    {
        public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Applying {pendingMigrations.Count()} Pending Migrations");
                await dbContext.Database.MigrateAsync();
            }
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath,"wwroot","Files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);
        }
    }
}
