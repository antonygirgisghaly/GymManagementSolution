
using GymMangment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Configrations
{
    public class TrainerConfigration : GymUserConfigration<Trainer> , IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(a => a.CreatedAt)
                  .HasColumnName("HireDate")
                  .HasDefaultValueSql("GETDATE()");
            base.Configure(builder);
        }
    }
}
