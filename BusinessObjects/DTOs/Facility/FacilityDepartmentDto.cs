using BusinessObjects.DTOs.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Facility
{
    public class FacilityDepartmentDto
    {
        public int? Id { get; set; }
        public int? FacilityId { get; set; }
        public int? DepartmentId { get; set; }

        public DepartmentDto? Department { get; set; }
    }
}
