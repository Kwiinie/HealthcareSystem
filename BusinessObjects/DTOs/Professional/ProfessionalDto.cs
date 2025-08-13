using BusinessObjects.DTOs.Service;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Professional
{
    public class ProfessionalDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ExpertiseId { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public string? Degree { get; set; }
        public string? Experience { get; set; }
        public string? WorkingHours { get; set; }
        public string? RequestStatus { get; set; }

        // User information
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImgUrl { get; set; }
        public string? Gender { get; set; }
        public string Status { get; set; }
        public string? ExpertiseName { get; set; } 
        public List<string> Specialties { get; set; } = new List<string>(); 

        public List<ServiceDto> PrivateServices { get; set; } = new List<ServiceDto>();
    }


}
