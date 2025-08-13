using BusinessObjects.Commons;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Payment : BaseEntity
{
    public string? PaymentMethod { get; set; }

    public string? TransactionId { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public decimal? Price { get; set; }

    public DateTime? PaymentDate { get; set; }
    public string? PaymentUrl { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
