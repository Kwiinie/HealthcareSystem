using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Appointments)
                   .WithOne(a => a.Payment)
                   .HasForeignKey(a => a.PaymentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(
                 new Payment
                 {
                     Id = 1,
                     PaymentMethod = "VnPay",
                     TransactionId = "VNP13579246",
                     PaymentStatus = PaymentStatus.Completed,
                     Price = 250000, // 250000 - Based on Private Service ID 1 price
                     PaymentDate = new DateTime(2025, 3, 15, 10, 30, 0),
                     PaymentUrl = "https://vnpay.vn/transaction/VNP13579246"
                 },
    new Payment
    {
        Id = 2,
        PaymentMethod = "VnPay",
        TransactionId = "VNP24681357",
        PaymentStatus = PaymentStatus.Completed,
        Price = 60000, // 600000 - Based on Private Service ID 11 price
        PaymentDate = new DateTime(2025, 3, 18, 14, 15, 0),
        PaymentUrl = "https://vnpay.vn/transaction/VNP24681357"
    },
    new Payment
    {
        Id = 3,
        PaymentMethod = "VnPay",
        TransactionId = "VNP98765432",
        PaymentStatus = PaymentStatus.Completed,
        Price = 325000, // 325000 - Based on Private Service ID 14 price
        PaymentDate = new DateTime(2025, 3, 20, 9, 45, 0),
        PaymentUrl = "https://vnpay.vn/transaction/VNP98765432"
    },
    new Payment
    {
        Id = 4,
        PaymentMethod = "VnPay",
        TransactionId = "VNP12345678",
        PaymentStatus = PaymentStatus.Completed,
        Price = 200000, // 200000 - Based on Private Service ID 7 price
        PaymentDate = new DateTime(2025, 3, 22, 11, 0, 0),
        PaymentUrl = "https://vnpay.vn/transaction/VNP12345678"
    },
    new Payment
    {
        Id = 5,
        PaymentMethod = "VnPay",
        TransactionId = "VNP87654321",
        PaymentStatus = PaymentStatus.Completed,
        Price = 350000, // 350000 - Based on Private Service ID 36 price
        PaymentDate = new DateTime(2025, 3, 25, 16, 30, 0),
        PaymentUrl = "https://vnpay.vn/transaction/VNP87654321"
    },

    // Pending Payments
    new Payment
    {
        Id = 6,
        PaymentMethod = "VnPay",
        TransactionId = "VNP24680135",
        PaymentStatus = PaymentStatus.Pending,
        Price = 100000, // 100000 - Based on Public Service ID 19 price
        PaymentDate = new DateTime(2025, 3, 29, 10, 45, 0),
        PaymentUrl = "https://vnpay.vn/transaction/VNP24680135"
    },
    new Payment
    {
        Id = 7,
        PaymentMethod = "VnPay",
        TransactionId = "VNP13572468",
        PaymentStatus = PaymentStatus.Pending,
        Price = 125000, // 125000 - Based on Public Service ID 29 price
        PaymentDate = new DateTime(2025, 3, 30, 15, 20, 0),
        PaymentUrl = "https://vnpay.vn/transaction/VNP13572468"
    },

    // Failed Payments
    new Payment
    {
        Id = 8,
        PaymentMethod = "VnPay",
        TransactionId = "VNP11223344",
        PaymentStatus = PaymentStatus.Failed,
        Price = 250000, // 250000 - Based on Public Service ID 36 price
        PaymentDate = new DateTime(2025, 3, 28, 14, 10, 0),
        PaymentUrl = "https://vnpay.vn/transaction/VNP11223344"
    },
    new Payment
    {
        Id = 9,
        PaymentMethod = "VnPay",
        TransactionId = "VNP99887766",
        PaymentStatus = PaymentStatus.Failed,
        Price = 100000, // 100000 - Based on Public Service ID 41 price
        PaymentDate = new DateTime(2025, 3, 29, 9, 30, 0),
        PaymentUrl = "https://vnpay.vn/transaction/VNP99887766"
    }
                );
        }
    }
}
