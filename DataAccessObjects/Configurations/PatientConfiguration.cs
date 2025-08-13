using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace DataAccessObjects.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(p => p.User)
           .WithOne(u => u.Patient)
           .HasForeignKey<Patient>(p => p.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Reviews)
                .WithOne(r => r.Patient)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Patient
                {
                    Id = 1,
                    UserId = 2,
                    Note = "Tôi có tiền sử bệnh tim mạch, đã từng phẫu thuật van tim năm 2020",
                },
    new Patient
    {
        Id = 2,
        UserId = 3,
        Note = "Tôi bị tiểu đường type 2 và huyết áp cao, đang điều trị thuốc ổn định",
    },

    new Patient
    {
        Id = 3,
        UserId = 6,
        Note = "Tôi có tiền sử viêm gan B, đang theo dõi định kỳ",
    },
    new Patient
    {
        Id = 4,
        UserId = 7,
        Note = "Tôi bị hen suyễn từ nhỏ, có dị ứng với bụi và phấn hoa",
    },
    new Patient
    {
        Id = 5,
        UserId = 10,
        Note = "Bệnh nhân bị thoái hóa cột sống, đang điều trị vật lý trị liệu",
    },
    new Patient
    {
        Id = 6,
        UserId = 11,
        Note = "Tôi có tiền sử sỏi thận, đã phẫu thuật lấy sỏi năm 2022",
    }
                );
        }
    }
}
