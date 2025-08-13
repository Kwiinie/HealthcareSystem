using BusinessObjects.Commons;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class PrivateService : BaseEntity
{
    public int? ProfessionalId { get; set; }

    public decimal? Price { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual Professional? Professional { get; set; }
}
