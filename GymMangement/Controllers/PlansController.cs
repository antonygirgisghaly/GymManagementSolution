using GymMangement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GymMangement.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext dbContext;
        public PlansController()
        {
            dbContext = new GymDbContext();
        }
        public async Task<IActionResult> Index()
        {
            var plan = await dbContext.Plan.ToListAsync();
            return View(plan);
        }
    }
}
