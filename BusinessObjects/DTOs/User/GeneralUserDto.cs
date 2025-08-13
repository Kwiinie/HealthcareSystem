 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dtos.User
{
    public class GeneralUserDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }

        public string Role { get; set; }
        public string Status { get; set; }
        public DateOnly Birthday { get; set; }
        public string ImgUrl { get; set; }
    }
}
