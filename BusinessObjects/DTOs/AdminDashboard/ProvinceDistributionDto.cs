using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.AdminDashboard
{
    public class ProvinceDistributionDto
    {
        public string Province { get; set; } = "";
        public int FacilityCount { get; set; }
        public int ProfessionalCount { get; set; }
    }

}
