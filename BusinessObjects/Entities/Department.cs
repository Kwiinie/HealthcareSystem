using BusinessObjects.Commons;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Department : BaseEntity
{

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<FacilityDepartment> FacilityDepartments { get; set; } = new List<FacilityDepartment>();
}
