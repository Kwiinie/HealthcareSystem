using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Payment
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string? PaymentMethod { get; set; }

        public string? TransactionId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public decimal? Price { get; set; }

        public DateTime? PaymentDate { get; set; }
        public string? PaymentUrl { get; set; }
        public int? AppointmentId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
