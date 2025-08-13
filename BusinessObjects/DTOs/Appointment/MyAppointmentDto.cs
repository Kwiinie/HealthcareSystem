using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Payment;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Appointment
{
    public class MyAppointmentDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? ProviderId { get; set; }
        public ProviderType ProviderType { get; set; }
        public ServiceDto Service { get; set; }

        public ProfessionalDto? Professional { get; set; }
        public SearchingFacilityDto? Facility { get; set; }
        public AppointmentStatus Status { get; set; }
        public PaymentDto Payment { get; set; }
        public string? TransactionId { get; set; }
        public string Description { get; set; }
    }
}
