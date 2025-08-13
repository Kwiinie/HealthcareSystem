using BusinessObjects.DTOs.Service;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Facility
{
    public class SearchingFacilityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly? OperationDay { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public FacilityStatus Status { get; set; }
        public string ImgUrl { get; set; }

        public string FacilityTypeName { get; set; } // FacilityType.Name
        public List<string> DepartmentNames { get; set; } = new List<string>();  // Department.Name
        public List<ServiceDto> PublicServices { get; set; } = new List<ServiceDto>(); // Service.Name, Price, Description
    }
}
