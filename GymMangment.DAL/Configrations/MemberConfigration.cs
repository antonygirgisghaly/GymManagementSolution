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
    public class MemberConfigration : GymUserConfigration<Member> , IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(a => a.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasColumnType("GETDATE()");
            base.Configure(builder);
        }
    }
}
