using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Configurations
{
    public class ExpertiseConfiguration : IEntityTypeConfiguration<Expertise>
    {
        public void Configure(EntityTypeBuilder<Expertise> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(e => e.Professionals)
           .WithOne(p => p.Expertise)
           .HasForeignKey(p => p.ExpertiseId)
           .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(
            new Expertise { Id = 1, Name = "Bác sĩ đa khoa", Description = "Tốt nghiệp đại học Y khoa hệ chính quy (6 năm)." },
            new Expertise { Id = 2, Name = "Bác sĩ y học cổ truyền", Description = "Tốt nghiệp đại học Y học cổ truyền (6 năm)." },
            new Expertise { Id = 3, Name = "Bác sĩ Răng - Hàm - Mặt", Description = "Tốt nghiệp đại học chuyên khoa Răng - Hàm - Mặt (6 năm)." },
            new Expertise { Id = 4, Name = "Bác sĩ Y học dự phòng", Description = "Tốt nghiệp đại học chuyên ngành Y học dự phòng (6 năm)." },
            new Expertise { Id = 5, Name = "Dược sĩ đại học", Description = "Tốt nghiệp đại học ngành Dược (5 năm)." },
            new Expertise { Id = 6, Name = "Cử nhân Điều dưỡng", Description = "Tốt nghiệp đại học ngành Điều dưỡng (4 năm)." },
            new Expertise { Id = 7, Name = "Bác sĩ nội trú", Description = "Đào tạo chuyên sâu 3 năm sau khi tốt nghiệp bác sĩ đa khoa." },
            new Expertise { Id = 8, Name = "Bác sĩ chuyên khoa I", Description = "Đào tạo sau đại học chuyên sâu trong lĩnh vực y khoa (2 năm)." },
            new Expertise { Id = 9, Name = "Bác sĩ chuyên khoa II", Description = "Cấp cao hơn chuyên khoa I, đào tạo tiếp 2-3 năm." },
            new Expertise { Id = 10, Name = "Thạc sĩ Y khoa", Description = "Học vị thạc sĩ ngành y khoa (2 năm)." },
            new Expertise { Id = 11, Name = "Tiến sĩ Y khoa", Description = "Học vị tiến sĩ y học, chuyên sâu nghiên cứu (3-5 năm)." },
            new Expertise { Id = 12, Name = "Phó Giáo sư - Tiến sĩ", Description = "Học hàm Phó Giáo sư, có nhiều nghiên cứu và đóng góp khoa học." },
            new Expertise { Id = 13, Name = "Giáo sư - Tiến sĩ", Description = "Học hàm Giáo sư, chuyên gia đầu ngành y tế." }
            );
        }
    }
}
