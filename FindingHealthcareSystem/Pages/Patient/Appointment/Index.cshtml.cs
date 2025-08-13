using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs.Appointment;
using BusinessObjects.DTOs.Payment;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Patient.Appointment
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        public IndexModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? ProviderType { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? PaymentStatus { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? PaymentFromDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? PaymentToDate { get; set; }
        public List<MyAppointmentDto> Appointments { get; set; } = new();
        public async Task<IActionResult> OnGetAsync()
        {
            var currentUserJson = HttpContext.Session.GetString("User");
            GeneralUserDto currentUser = null;
            if (!string.IsNullOrEmpty(currentUserJson))
            {
                currentUser = JsonConvert.DeserializeObject<GeneralUserDto>(currentUserJson);
            }
            var allAppointments = await _appointmentService.GetMyAppointment(currentUser.Id);
            var filtered = allAppointments.AsQueryable();

            // Filter by appointment status
            if (!string.IsNullOrEmpty(Status) && Enum.TryParse<AppointmentStatus>(Status, out var statusEnum))
            {
                filtered = filtered.Where(a => a.Status == statusEnum);
            }

            // Filter by provider type
            if (!string.IsNullOrEmpty(ProviderType) && Enum.TryParse<ProviderType>(ProviderType, out var providerTypeEnum))
            {
                filtered = filtered.Where(a => a.ProviderType == providerTypeEnum);
            }

            // Filter by appointment date range
            if (FromDate.HasValue)
            {
                filtered = filtered.Where(a => a.Date.Date >= FromDate.Value.Date);
            }
            if (ToDate.HasValue)
            {
                filtered = filtered.Where(a => a.Date.Date <= ToDate.Value.Date);
            }

            // Filter by payment status
            if (!string.IsNullOrEmpty(PaymentStatus) && Enum.TryParse<PaymentStatus>(PaymentStatus, out var paymentStatusEnum))
            {
                filtered = filtered.Where(a => a.Payment != null && a.Payment.PaymentStatus == paymentStatusEnum);
            }

            Appointments = filtered.ToList();
            return Page();
        }
    }
}

