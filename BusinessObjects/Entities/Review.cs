using BusinessObjects.Commons;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Review : BaseEntity
{
    public int? ProviderId { get; set; }

    public ProviderType ProviderType { get; set; }

    public int? PatientId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? Date { get; set; }

    public virtual Patient? Patient { get; set; }
}
