using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccessObjects.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Payment)
                .WithMany()
                .HasForeignKey(a => a.PaymentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(a => a.MedicalRecords)
               .WithOne(mr => mr.Appointment)
               .HasForeignKey(mr => mr.AppointmentId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
    // 1. Completed
    new Appointment
    {
        Id = 1,
        PatientId = 1, // Trần Thị Bích
        ProviderId = 1, // Phạm Minh Đức
        ProviderType = ProviderType.Professional,
        ServiceId = 1,
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.Completed,
        PaymentId = 1,
        ExpectedStart = new DateTime(2025, 3, 15, 9, 0, 0),
        CheckedInAt = new DateTime(2025, 3, 15, 8, 50, 0),
        StartAt = new DateTime(2025, 3, 15, 9, 5, 0),
        EndAt = new DateTime(2025, 3, 15, 9, 30, 0),
        TicketNo = 1
    },

    // 2. Completed
    new Appointment
    {
        Id = 2,
        PatientId = 2, // Lê Văn Cường
        ProviderId = 3, // Ngô Thanh Tùng
        ProviderType = ProviderType.Professional,
        ServiceId = 11,
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.Completed,
        PaymentId = 2,
        ExpectedStart = new DateTime(2025, 3, 18, 14, 0, 0),
        CheckedInAt = new DateTime(2025, 3, 18, 13, 50, 0),
        StartAt = new DateTime(2025, 3, 18, 14, 5, 0),
        EndAt = new DateTime(2025, 3, 18, 14, 45, 0),
        TicketNo = 2
    },

    // 3. Scheduled (chưa đến ngày)
    new Appointment
    {
        Id = 3,
        PatientId = 3, // Hoàng Thị Mai
        ProviderId = 4, // Lý Thị Hoa
        ProviderType = ProviderType.Professional,
        ServiceId = 14,
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.Scheduled,
        PaymentId = 3,
        ExpectedStart = new DateTime(2025, 4, 5, 10, 0, 0),
        TicketNo = 3
    },

    // 4. CheckedIn
    new Appointment
    {
        Id = 4,
        PatientId = 4, // Đỗ Quang Nam
        ProviderId = 2, // Vũ Thị Hương
        ProviderType = ProviderType.Professional,
        ServiceId = 7,
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.CheckedIn,
        PaymentId = 4,
        ExpectedStart = new DateTime(2025, 4, 10, 15, 0, 0),
        CheckedInAt = new DateTime(2025, 4, 10, 14, 50, 0),
        TicketNo = 4
    },

    // 5. InExam
    new Appointment
    {
        Id = 5,
        PatientId = 5, // Dương Văn Khải
        ProviderId = 17,
        ProviderType = ProviderType.Professional,
        ServiceId = 9,
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.InExam,
        PaymentId = null,
        ExpectedStart = new DateTime(2025, 4, 12, 9, 30, 0),
        CheckedInAt = new DateTime(2025, 4, 12, 9, 20, 0),
        StartAt = new DateTime(2025, 4, 12, 9, 35, 0),
        TicketNo = 5
    },

    // 6. CancelledByPatient
    new Appointment
    {
        Id = 6,
        PatientId = 6, // Trịnh Thu Phương
        ProviderId = 15, // Phạm Thị Thùy Trang
        ProviderType = ProviderType.Professional,
        ServiceId = 36,
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.CancelledByPatient,
        PaymentId = 5,
        ExpectedStart = new DateTime(2025, 4, 2, 13, 0, 0)
    },

    // 7. CancelledByDoctor
    new Appointment
    {
        Id = 7,
        PatientId = 1, // Trần Thị Bích
        ProviderId = 1, // Bệnh viện Bạch Mai
        ProviderType = ProviderType.Facility,
        ServiceId = 1,
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.CancelledByDoctor,
        PaymentId = null,
        ExpectedStart = new DateTime(2025, 3, 25, 8, 0, 0)
    },

    // 8. NoShow
    new Appointment
    {
        Id = 8,
        PatientId = 2, // Lê Văn Cường
        ProviderId = 7, // Bệnh viện Mắt TW
        ProviderType = ProviderType.Facility,
        ServiceId = 19,
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.NoShow,
        PaymentId = 6,
        ExpectedStart = new DateTime(2025, 4, 15, 10, 30, 0),
        TicketNo = 6
    }
);

        }
    }
}
