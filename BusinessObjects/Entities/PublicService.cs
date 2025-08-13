using BusinessObjects.Commons;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class PublicService : BaseEntity
{
    public int? FacilityId { get; set; }

    public decimal? Price { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual Facility? Facility { get; set; }
}
