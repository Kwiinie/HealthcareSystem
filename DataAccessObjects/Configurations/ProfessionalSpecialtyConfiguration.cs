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
    public class ProfessionalSpecialtyConfiguration : IEntityTypeConfiguration<ProfessionalSpecialty>
    {
        public void Configure(EntityTypeBuilder<ProfessionalSpecialty> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.HasOne(ps => ps.Professional)
                .WithMany(p => p.ProfessionalSpecialties) 
                .HasForeignKey(ps => ps.ProfessionalId) 
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(ps => ps.Specialty)
                .WithMany(s => s.ProfessionalSpecialties)  
                .HasForeignKey(ps => ps.SpecialtyId)  
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
            new ProfessionalSpecialty { Id = 9, ProfessionalId = 6, SpecialtyId = 6 },  // Chuyên khoa Sản phụ khoa
new ProfessionalSpecialty { Id = 10, ProfessionalId = 7, SpecialtyId = 4 },  // Chuyên khoa Thần kinh
new ProfessionalSpecialty { Id = 11, ProfessionalId = 8, SpecialtyId = 1 },  // Chuyên khoa Nội
new ProfessionalSpecialty { Id = 12, ProfessionalId = 9, SpecialtyId = 11 }, // Chuyên khoa Phục hồi chức năng
new ProfessionalSpecialty { Id = 13, ProfessionalId = 10, SpecialtyId = 14 }, // Chuyên khoa Nội tiết
new ProfessionalSpecialty { Id = 14, ProfessionalId = 11, SpecialtyId = 13 }, // Chuyên khoa Hô hấp
new ProfessionalSpecialty { Id = 15, ProfessionalId = 12, SpecialtyId = 8 },  // Chuyên khoa Ung bướu
new ProfessionalSpecialty { Id = 16, ProfessionalId = 13, SpecialtyId = 7 },  // Chuyên khoa Nhi
new ProfessionalSpecialty { Id = 17, ProfessionalId = 14, SpecialtyId = 15 }, // Chuyên khoa Nha khoa
new ProfessionalSpecialty { Id = 18, ProfessionalId = 15, SpecialtyId = 5 },  // Chuyên khoa Da liễu
new ProfessionalSpecialty { Id = 19, ProfessionalId = 16, SpecialtyId = 2 },  // Chuyên khoa Ngoại
new ProfessionalSpecialty { Id = 20, ProfessionalId = 17, SpecialtyId = 12 }, // Chuyên khoa Y học cổ truyền
new ProfessionalSpecialty { Id = 21, ProfessionalId = 18, SpecialtyId = 10 }, // Chuyên khoa Tai Mũi Họng
new ProfessionalSpecialty { Id = 22, ProfessionalId = 19, SpecialtyId = 9 },  // Chuyên khoa Mắt
new ProfessionalSpecialty { Id = 23, ProfessionalId = 20, SpecialtyId = 1 },  // Chuyên khoa Nội
new ProfessionalSpecialty { Id = 24, ProfessionalId = 12, SpecialtyId = 3 },  // Additional specialty for Giáo sư (Tim mạch)
new ProfessionalSpecialty { Id = 25, ProfessionalId = 16, SpecialtyId = 1 }   // Additional specialty for BS CK II (Nội)
            );
        }
    }
}
