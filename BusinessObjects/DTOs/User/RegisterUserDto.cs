using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User
{
    public class RegisterUserDto
    {
        public string Fullname { get; set; }
        public string Email { get; set; }

       
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public DateOnly? Birthday { get; set; }
        public string Gender { get; set; }
        public string? Province { get; set; }
        public int? ExpertiseId { get; set; }

        public string? ImgUrl { get; set; }
        public string Note { get; set; }

        public string? District { get; set; }
        public string? WorkingHours { get; set; } /// giờ làm vieecjj khoảng thời gian 

        public string? Ward { get; set; }
        public string? Address { get; set; }
        public string? Degree { get; set; }
        public string? Experience { get; set; }

        public List<int> SpecialtyIds { get; set; } = new();
    }

}
