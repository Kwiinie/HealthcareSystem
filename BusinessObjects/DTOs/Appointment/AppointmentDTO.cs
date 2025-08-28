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
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? ProviderId { get; set; }
        public ProviderType ProviderType { get; set; }
        public int? ServiceId { get; set; }
        public ServiceType ServiceType { get; set; }
        public AppointmentStatus Status { get; set; }
        public int? PaymentId { get; set; }
        public AppointmentSource Source { get; set; }

        public DateTime ExpectedStart { get; set; }       
        public DateTime? CheckedInAt { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public DateTime? Date { get; set; }

        public int? TicketNo { get; set; }

        public string? Description { get; set; }

        public PatientDTO? Patient { get; set; }
        public PaymentDto? Payment { get; set; }
        public ServiceDto? PrivateService { get; set; }
        public ServiceDto? PublicService { get; set; }

        
    }
}
