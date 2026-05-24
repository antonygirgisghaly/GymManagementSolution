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
    public class GymUserConfigration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);
            builder.Property(x => x.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);
            builder.HasIndex(x => x.Email)
                   .IsUnique();
            builder.HasIndex(x => x.Phone)
                   .IsUnique();
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email Like '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "Phone Like '010% Or Phone Like '011%' Or Phone Like '012% Or Phone Like '015%'");
            });
            builder.OwnsOne(x => x.Address,
                a =>
                { a.Property(b => b.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(30);
                a.Property(b => b.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(30);
                 } );
        }
    }
}
