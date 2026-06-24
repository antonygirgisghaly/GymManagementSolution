using Microsoft.EntityFrameworkCore;
using GymMangment.DAL.Data.Configrations;
using System.Reflection;
using GymMangment.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace GymMangment.DAL.Data.DbContexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=GymSystem;Trusted_Connection=true;TrustServerCertificate=true");
        //}
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(ap =>
            {
                ap.Property(x => x.FirstName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

                ap.Property(x => x.LastName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);
            });
        }

        public DbSet<Plan> Plan { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Catagory> Catagories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
       
    }
}
