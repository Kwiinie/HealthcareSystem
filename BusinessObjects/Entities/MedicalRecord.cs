using BusinessObjects.Commons;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class MedicalRecord : BaseEntity
{
    public int? AppointmentId { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? Note { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
