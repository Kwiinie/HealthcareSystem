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
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(s => s.ProfessionalSpecialties)
           .WithOne(ps => ps.Specialty) 
           .HasForeignKey(ps => ps.SpecialtyId) 
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Specialty
                {
                    Id = 1,
                    Name = "Chuyên khoa Nội",
                    Description = "Chuyên ngành điều trị các bệnh lý nội bộ của cơ thể như bệnh tim mạch, tiêu hóa, hô hấp, thận."
                },
            new Specialty
            {
                Id = 2,
                Name = "Chuyên khoa Ngoại",
                Description = "Chuyên ngành liên quan đến phẫu thuật và điều trị các bệnh lý cần can thiệp phẫu thuật."
            },
            new Specialty
            {
                Id = 3,
                Name = "Chuyên khoa Tim mạch",
                Description = "Chuyên ngành chuyên sâu về bệnh lý tim mạch, bao gồm các bệnh liên quan đến tim và mạch máu."
            },
            new Specialty
            {
                Id = 4,
                Name = "Chuyên khoa Thần kinh",
                Description = "Chuyên ngành chẩn đoán và điều trị các bệnh liên quan đến hệ thần kinh như đột quỵ, động kinh."
            },
            new Specialty
            {
                Id = 5,
                Name = "Chuyên khoa Da liễu",
                Description = "Chuyên ngành chăm sóc và điều trị các bệnh lý về da như mụn, eczema, bệnh vảy nến."
            },
            new Specialty
            {
                Id = 6,
                Name = "Chuyên khoa Sản phụ khoa",
                Description = "Chuyên ngành điều trị các bệnh lý liên quan đến hệ sinh sản và chăm sóc sức khỏe phụ nữ."
            },
            new Specialty
            {
                Id = 7,
                Name = "Chuyên khoa Nhi",
                Description = "Chuyên ngành chăm sóc sức khỏe và điều trị bệnh lý cho trẻ em."
            },
            new Specialty
            {
                Id = 8,
                Name = "Chuyên khoa Ung bướu",
                Description = "Chuyên ngành điều trị và quản lý các bệnh lý ung thư."
            },
            new Specialty
            {
                Id = 9,
                Name = "Chuyên khoa Mắt",
                Description = "Chuyên ngành điều trị và chăm sóc các bệnh lý về mắt, bao gồm đục thủy tinh thể, tật khúc xạ."
            },
            new Specialty
            {
                Id = 10,
                Name = "Chuyên khoa Tai Mũi Họng",
                Description = "Chuyên ngành liên quan đến các bệnh lý tai, mũi, họng và các cấu trúc liên quan."
            },
            new Specialty
            {
                Id = 11,
                Name = "Chuyên khoa Phục hồi chức năng",
                Description = "Chuyên ngành tập trung vào phục hồi sức khỏe cho bệnh nhân sau phẫu thuật, tai nạn, hoặc các bệnh lý nghiêm trọng."
            },
            new Specialty
            {
                Id = 12,
                Name = "Chuyên khoa Y học cổ truyền",
                Description = "Chuyên ngành sử dụng các phương pháp y học cổ truyền như châm cứu, bấm huyệt để điều trị bệnh."
            },
            new Specialty
            {
                Id = 13,
                Name = "Chuyên khoa Hô hấp",
                Description = "Chuyên ngành nghiên cứu và điều trị các bệnh lý về hô hấp như viêm phổi, hen suyễn."
            },
            new Specialty
            {
                Id = 14,
                Name = "Chuyên khoa Nội tiết",
                Description = "Chuyên ngành điều trị các bệnh lý liên quan đến nội tiết tố như tiểu đường, rối loạn tuyến giáp."
            },
            new Specialty
            {
                Id = 15,
                Name = "Chuyên khoa Nha khoa",
                Description = "Chuyên ngành chăm sóc sức khỏe răng miệng, bao gồm điều trị sâu răng, chỉnh hình răng miệng."
            }
        );
            
        }
    }
}
