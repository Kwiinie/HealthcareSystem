using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Service
{
    public class ServiceDto
    {
        public int? Id { get; set; }
        public int? FacilityId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
