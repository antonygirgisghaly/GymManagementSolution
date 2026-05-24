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
    public class CatagoryConfigration : IEntityTypeConfiguration<Catagory>
    {
        public void Configure(EntityTypeBuilder<Catagory> builder)
        {
            builder.Property(p => p.CatagoryName)
                   .HasColumnType("varchar")
                   .HasMaxLength(20);
            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.HasData(new Catagory { Id = 1, CatagoryName = "Cardio" },
                            new Catagory { Id = 2, CatagoryName = "Strength" },
                            new Catagory { Id = 3, CatagoryName = "Yoga" },
                            new Catagory { Id = 4, CatagoryName = "Boxing" },
                            new Catagory { Id = 5, CatagoryName = "CrossFit" });
        }
    }
}
