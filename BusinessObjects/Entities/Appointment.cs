using BusinessObjects.Commons;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Appointment : BaseEntity
{
    public int? PatientId { get; set; }

    public int? ProviderId { get; set; }

    public ProviderType ProviderType { get; set; }

    public int? ServiceId { get; set; }

    public ServiceType ServiceType { get; set; }

    public AppointmentStatus Status { get; set; }

    public int? PaymentId { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Patient? Patient { get; set; }

    public virtual Payment? Payment { get; set; }

    public Professional? Professional { get; set; }
    public Facility? Facility { get; set; }
    public PrivateService? PrivateService { get; set; }
    public PublicService? PublicService { get; set; }
}
