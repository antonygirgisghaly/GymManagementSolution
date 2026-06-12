using GymMangment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace GymMangment.DAL.Data.Configrations
{
    public class PlanConfigration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(p => p.Description)
                   .HasMaxLength(200);
            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");
            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.ToTable(tb => tb.HasCheckConstraint("PlanDuirationCheck", "DuirationDays between 1 and 365"));
            builder.HasData(
        new Plan
        {
            Id = 1,
            Name = "Basic Plan",
            Price = 300,
            DuirationDays = 30, 
            Description = "Access to gym equipment during staffed hours",
            IsActive = true
        },
        new Plan
        {
            Id = 2,
            Name = "Standard Plan",
            Price = 500,
            DuirationDays = 60,
            Description = "Includes gym equipment and 2 group classes per week",
            IsActive = false 
        },
        new Plan
        {
            Id = 3,
            Name = "Premium Plan",
            Price = 900,
            DuirationDays = 90,
            Description = "Unlimited access to equipment, classes, and sauna",
            IsActive = false 
        },
        new Plan
        {
            Id = 4,
            Name = "Annual Plan",
            Price = 3000,
            DuirationDays = 360,
            Description = "Full year access with personal trainer sessions",
            IsActive = true
        }
    );
        }

    }
}
