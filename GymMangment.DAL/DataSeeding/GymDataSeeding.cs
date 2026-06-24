using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GymMangment.DAL.Data.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(
            GymDbContext dbContext,
            string seedFolderPath,
            ILogger logger,
            CancellationToken ct = default)
        {
            try
            {
                if (!await dbContext.Plan.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>(
                        seedFolderPath,
                        "plans.json");

                    if (plans.Any())
                    {
                        dbContext.Plan.AddRange(plans);

                        var result =
                            await dbContext.SaveChangesAsync();

                        logger.LogInformation(
                            $"Plans Seeded With Count = {result}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "An Error Occurred While Seeding Data");
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(
            string folderPath,
            string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                    $"Seed Data File Not Found : {filePath}");

            var data = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<T>>(data)
                   ?? new List<T>();
        }
    }
}