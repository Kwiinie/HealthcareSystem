using BusinessObjects.Commons;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Attachment : BaseEntity
{

    public int? MedicalRecordId { get; set; }

    public string? Url { get; set; }

    public virtual MedicalRecord? MedicalRecord { get; set; }
}
