using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Configurations
{
    public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasKey(mr => mr.Id);

            builder.HasOne(mr => mr.Appointment)
                   .WithMany(a => a.MedicalRecords)
                   .HasForeignKey(mr => mr.AppointmentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(mr => mr.Attachments)
           .WithOne(a => a.MedicalRecord)  
           .HasForeignKey(a => a.MedicalRecordId) 
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
