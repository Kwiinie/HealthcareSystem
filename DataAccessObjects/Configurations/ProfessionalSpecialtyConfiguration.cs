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
    // ---- Professional 1 (CKI Nội) ----
    new ProfessionalSpecialty { Id = 1, ProfessionalId = 1, SpecialtyId = 1 }, // Nội
    new ProfessionalSpecialty { Id = 2, ProfessionalId = 1, SpecialtyId = 3 }, // Tim mạch (phụ)

    // ---- Professional 2 (Y học cổ truyền) ----
    new ProfessionalSpecialty { Id = 3, ProfessionalId = 2, SpecialtyId = 12 }, // YHCT

    // ---- Professional 4 (CKII Tim mạch) ----
    new ProfessionalSpecialty { Id = 4, ProfessionalId = 4, SpecialtyId = 3 }, // Tim mạch
    new ProfessionalSpecialty { Id = 14, ProfessionalId = 4, SpecialtyId = 1 }, // Nội (phụ)

    // ---- Professional 6 (ThS Sản) ----
    new ProfessionalSpecialty { Id = 5, ProfessionalId = 6, SpecialtyId = 6 }, // Sản phụ khoa

    // ---- Professional 7 (PGS TS Thần kinh) ----
    new ProfessionalSpecialty { Id = 6, ProfessionalId = 7, SpecialtyId = 4 }, // Thần kinh

    // ---- Professional 8 (Đa khoa) ----
    new ProfessionalSpecialty { Id = 7, ProfessionalId = 8, SpecialtyId = 1 }, // Nội

    // ---- Professional 13 (Nội trú Nhi) ----
    new ProfessionalSpecialty { Id = 8, ProfessionalId = 13, SpecialtyId = 7 }, // Nhi

    // ---- Professional 15 (CKI Da liễu) ----
    new ProfessionalSpecialty { Id = 9, ProfessionalId = 15, SpecialtyId = 5 }, // Da liễu

    // ---- Professional 17 (YHCT) ----
    new ProfessionalSpecialty { Id = 10, ProfessionalId = 17, SpecialtyId = 12 }, // YHCT

    // ---- Professional 18 (TS TMH) ----
    new ProfessionalSpecialty { Id = 11, ProfessionalId = 18, SpecialtyId = 10 }, // Tai Mũi Họng

    // ---- Professional 19 (ThS Mắt) ----
    new ProfessionalSpecialty { Id = 12, ProfessionalId = 19, SpecialtyId = 9 }, // Mắt

    // ---- Professional 20 (Đa khoa) ----
    new ProfessionalSpecialty { Id = 13, ProfessionalId = 20, SpecialtyId = 1 }  // Nội
);

        }
    }
}
