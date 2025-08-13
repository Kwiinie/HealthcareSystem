using BusinessObjects.DTOs.Payment;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Appointment
{
    public class AppointmentDTO
    {
        [NotMapped]
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        public int? PatientId { get; set; }

        [NotMapped]
        public PatientDTO Patient { get; set; }

        public int? ProviderId { get; set; }
        public string? Diagnose { get; set; }
        public ProviderType ProviderType { get; set; }
        public int? ServiceId { get; set; }
        public ServiceType ServiceType { get; set; }
        public AppointmentStatus Status { get; set; }
        public int? PaymentId { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public PaymentDto Payment { get; set; }
        [NotMapped]
        public ServiceDto PrivateService { get; set; }
        [NotMapped]
        public ServiceDto PublicService { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                DateTime today = DateTime.Now;
                int age = today.Year - Patient.User.Birthday.Year;
                if (Patient.User.Birthday.Year > today.Year)
                {
                    age--;
                }
                return age;
            }
        }
    }
}
