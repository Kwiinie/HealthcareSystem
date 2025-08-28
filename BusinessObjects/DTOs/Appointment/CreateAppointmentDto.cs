using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
        public int? ProviderId { get; set; }
        public ProviderType ProviderType { get; set; }
        public int? ServiceId { get; set; }
        public ServiceType ServiceType { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Description { get; set; }
    }
}
