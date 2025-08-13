using BusinessObjects.Commons;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class ProfessionalSpecialty : BaseEntity
{
    public int? ProfessionalId { get; set; }

    public int? SpecialtyId { get; set; }

    public virtual Professional? Professional { get; set; }

    public virtual Specialty? Specialty { get; set; }
}
