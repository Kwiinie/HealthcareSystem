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
                new Appointment
                {
                    Id = 1,
                    PatientId = 1, // Trần Thị Bích
                    ProviderId = 1, // Phạm Minh Đức (Bác sĩ chuyên khoa I)
                    ProviderType = ProviderType.Professional,
                    ServiceId = 1, // Khám và tư vấn bệnh lý nội khoa
                    ServiceType = ServiceType.Private,
                    Status = AppointmentStatus.Completed,
                    PaymentId = 1, // VnPay Completed
                    Date = new DateTime(2025, 3, 15, 9, 0, 0)
                },
    new Appointment
    {
        Id = 2,
        PatientId = 2, // Lê Văn Cường
        ProviderId = 3, // Ngô Thanh Tùng (Bác sĩ Răng - Hàm - Mặt)
        ProviderType = ProviderType.Professional,
        ServiceId = 11, // Điều trị tủy răng
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.Completed,
        PaymentId = 2, // VnPay Completed
        Date = new DateTime(2025, 3, 18, 14, 0, 0)
    },

    // Confirmed appointments
    new Appointment
    {
        Id = 3,
        PatientId = 3, // Hoàng Thị Mai
        ProviderId = 4, // Lý Thị Hoa (Bác sĩ chuyên khoa II)
        ProviderType = ProviderType.Professional,
        ServiceId = 14, // Khám và tư vấn bệnh lý tim mạch
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.Confirmed,
        PaymentId = 3, // VnPay Completed
        Date = new DateTime(2025, 4, 5, 10, 0, 0)
    },

    // Pending appointments
    new Appointment
    {
        Id = 4,
        PatientId = 4, // Đỗ Quang Nam
        ProviderId = 2, // Vũ Thị Hương (Bác sĩ y học cổ truyền)
        ProviderType = ProviderType.Professional,
        ServiceId = 7, // Châm cứu điều trị đau nhức
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.Pending,
        PaymentId = 4, // VnPay Completed
        Date = new DateTime(2025, 4, 10, 15, 0, 0)
    },

    // Awaiting Payment appointments
    new Appointment
    {
        Id = 5,
        PatientId = 5, // Dương Văn Khải
        ProviderId = 17, // Nguyễn Thị Bích Ngọc (Y học cổ truyền)
        ProviderType = ProviderType.Professional,
        ServiceId = 9, // Cấy chỉ điều trị đau thần kinh tọa
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.AwaitingPayment,
        PaymentId = null,
        Date = new DateTime(2025, 4, 12, 9, 30, 0)
    },

    // Cancelled appointments
    new Appointment
    {
        Id = 6,
        PatientId = 6, // Trịnh Thu Phương
        ProviderId = 15, // Phạm Thị Thùy Trang (Da liễu)
        ProviderType = ProviderType.Professional,
        ServiceId = 36, // Điều trị mụn và sẹo
        ServiceType = ServiceType.Private,
        Status = AppointmentStatus.Cancelled,
        PaymentId = 5, // VnPay Completed (refund would be processed)
        Date = new DateTime(2025, 4, 2, 13, 0, 0)
    },

    // Appointments with Public Services (Facility as Provider)

    // Expired appointments
    new Appointment
    {
        Id = 7,
        PatientId = 1, // Trần Thị Bích
        ProviderId = 1, // Bệnh viện Bạch Mai
        ProviderType = ProviderType.Facility,
        ServiceId = 1, // Khám nội khoa tổng quát
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.Expired,
        PaymentId = null,
        Date = new DateTime(2025, 3, 25, 8, 0, 0)
    },

    // Pending with pending payment
    new Appointment
    {
        Id = 8,
        PatientId = 2, // Lê Văn Cường
        ProviderId = 7, // Bệnh viện Mắt Trung ương
        ProviderType = ProviderType.Facility,
        ServiceId = 19, // Khám mắt tổng quát
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.AwaitingPayment,
        PaymentId = 6, // VnPay Pending
        Date = new DateTime(2025, 4, 15, 10, 30, 0)
    },

    // Rejected appointments
    new Appointment
    {
        Id = 9,
        PatientId = 3, // Hoàng Thị Mai
        ProviderId = 17, // Bệnh viện Đa khoa Phương Châu
        ProviderType = ProviderType.Facility,
        ServiceId = 9, // Sinh mổ theo yêu cầu
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.Rejected,
        PaymentId = null,
        Date = new DateTime(2025, 4, 20, 9, 0, 0)
    },

    // Rescheduled appointments
    new Appointment
    {
        Id = 10,
        PatientId = 4, // Đỗ Quang Nam
        ProviderId = 3, // Bệnh viện Đa khoa Quốc tế Vinmec Times City
        ProviderType = ProviderType.Facility,
        ServiceId = 39, // Gói khám sức khỏe tổng quát cơ bản
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.Rescheduled,
        PaymentId = null,
        Date = new DateTime(2025, 4, 18, 14, 0, 0)
    },

    // Failed payment
    new Appointment
    {
        Id = 11,
        PatientId = 5, // Dương Văn Khải
        ProviderId = 6, // Bệnh viện K Tân Triều
        ProviderType = ProviderType.Facility,
        ServiceId = 36, // Khám ung bướu
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.AwaitingPayment,
        PaymentId = 8, // VnPay Failed
        Date = new DateTime(2025, 4, 22, 9, 0, 0)
    },

    // Pending payment
    new Appointment
    {
        Id = 12,
        PatientId = 6, // Trịnh Thu Phương
        ProviderId = 5, // Bệnh viện Đa khoa Trung ương Cần Thơ
        ProviderType = ProviderType.Facility,
        ServiceId = 29, // Vật lý trị liệu
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.AwaitingPayment,
        PaymentId = 7, // VnPay Pending
        Date = new DateTime(2025, 4, 25, 15, 30, 0)
    },

    // Failed payment
    new Appointment
    {
        Id = 13,
        PatientId = 1, // Trần Thị Bích
        ProviderId = 12, // Bệnh viện Đa khoa tỉnh Lào Cai
        ProviderType = ProviderType.Facility,
        ServiceId = 41, // Khám và điều trị y học cổ truyền
        ServiceType = ServiceType.Public,
        Status = AppointmentStatus.AwaitingPayment,
        PaymentId = 9, // VnPay Failed
        Date = new DateTime(2025, 4, 28, 8, 30, 0)
    }
           );
        }
    }
}
