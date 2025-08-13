using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs.Appointment;
using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Payment;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.Services;
using System.Globalization;

namespace FindingHealthcareSystem.Pages.Patient.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly IProfessionalService _professionalService;
        private readonly IFacilityService _facilityService;
        private readonly IAppointmentService _appointmentService;
        private readonly IPaymentService _paymentService;

        public CreateModel(IProfessionalService professionalService, IFacilityService facilityService, IAppointmentService appointmentService, IPaymentService paymentService)
        {
            _facilityService = facilityService;
            _professionalService = professionalService;
            _appointmentService = appointmentService;
            _paymentService = paymentService;
        }

        /// <summary>
        /// PROPERTIES
        /// </summary>
        public List<string> TimeSlots { get; set; } = new List<string>();
        public int? ProviderId { get; set; }
        public string ProviderType { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public string SelectedTimeSlot { get; set; }
        [BindProperty]
        public int SelectedServiceId { get; set; }
        public ProfessionalDto? professional { get; set; }
        public SearchingFacilityDto? facility { get; set; }
        public List<ServiceDto> services { get; set; } = new List<ServiceDto>();

        public async Task<IActionResult> OnGetAsync(int? ProviderId, string ProviderType, string SelectedDate, string SelectedTimeSlot = null, int SelectedServiceId = 0)
        {
            this.ProviderId = ProviderId;
            this.ProviderType = ProviderType;

            if (!string.IsNullOrEmpty(SelectedDate) && DateTime.TryParse(SelectedDate, out DateTime parsedDate))
            {
                this.SelectedDate = parsedDate;
            }

            this.SelectedTimeSlot = SelectedTimeSlot;
            this.SelectedServiceId = SelectedServiceId;

            string workingHours = null;
            if (ProviderId.HasValue)
            {
                if (ProviderType == "Professional")
                {
                    professional = await _professionalService.GetById(ProviderId.Value);
                    services = professional?.PrivateServices ?? new List<ServiceDto>();
                    workingHours = professional?.WorkingHours;
                }
                else if (ProviderType == "Facility")
                {
                    facility = await _facilityService.GetFacilityById(ProviderId.Value);
                    services = facility?.PublicServices ?? new List<ServiceDto>();
                    workingHours = "7:00 - 16:00"; 
                }

                TimeSlots = GenerateTimeSlots(workingHours, this.SelectedDate);
                var bookedSlots = await GetBookedSlots(ProviderId.Value, ProviderType, this.SelectedDate);
                ViewData["BookedSlots"] = bookedSlots;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var providerId = int.Parse(Request.Form["ProviderId"]);
            var providerType = Request.Form["ProviderType"];
            var selectedDateStr = Request.Form["SelectedDate"];
            var selectedTimeSlot = Request.Form["SelectedTimeSlot"];
            var priceStr = Request.Form["SelectedServicePrice"];
            if (!decimal.TryParse(priceStr, out var depositAmount))
            {
                ModelState.AddModelError(string.Empty, "Giá dịch vụ không hợp lệ.");
                return Page();
            }

            var currentUserJson = HttpContext.Session.GetString("User");
            GeneralUserDto currentUser = null;

            if (!string.IsNullOrEmpty(currentUserJson))
            {
                currentUser = JsonConvert.DeserializeObject<GeneralUserDto>(currentUserJson);
            }

            if (!DateTime.TryParse(selectedDateStr, out DateTime selectedDate))
            {
                ModelState.AddModelError(string.Empty, "Invalid date selected.");
                return Page();
            }

            var selectedTimeSlotString = selectedTimeSlot.ToString();
            var timeSlotParts = selectedTimeSlotString.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (timeSlotParts.Length != 2 || !TimeSpan.TryParse(timeSlotParts[0], out TimeSpan startTime) || !TimeSpan.TryParse(timeSlotParts[1], out TimeSpan endTime))
            {
                ModelState.AddModelError(string.Empty, "Invalid time slot format.");
                return Page();
            }

            DateTime appointmentDateTime = selectedDate.Date.Add(startTime);

            var createAppointmentDto = new CreateAppointmentDto
            {
                Date = appointmentDateTime,
                PatientId = currentUser.Id,
                ProviderId = providerId,
                ProviderType = Enum.Parse<ProviderType>(providerType),
                ServiceId = SelectedServiceId,
            };

            var result = await _appointmentService.AddAsync(createAppointmentDto);


            

            if (result.Success)
            {

                var paymentRequest = new PaymentRequestDto
                {
                    AppointmentId = result.Data.Id.Value,
                    Amount = (float)depositAmount
                };

                string paymentUrl = await _paymentService.CreatePaymentAsync(paymentRequest, HttpContext);
                return Redirect(paymentUrl);

            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return Page();
            }
        }


        /// <summary>
        /// THIS FOR GENERATING ALL TIME SLOTS BASED ON WORKING HOURS
        /// </summary>
        private List<string> GenerateTimeSlots(string workingHours, DateTime selectedDate)
        {
            List<string> slots = new List<string>();

            if (selectedDate.DayOfWeek == DayOfWeek.Sunday)
                return slots;

            if (string.IsNullOrEmpty(workingHours))
                return slots;

            string[] hours = workingHours.Split('-');
            if (hours.Length != 2)
                return slots;

            TimeSpan startTime;
            TimeSpan endTime;

            if (!TimeSpan.TryParseExact(hours[0].Trim(), "h\\:mm", CultureInfo.InvariantCulture, out startTime))
            {
                if (!TimeSpan.TryParse(hours[0].Trim(), out startTime))
                    return slots;
            }

            if (!TimeSpan.TryParseExact(hours[1].Trim(), "h\\:mm", CultureInfo.InvariantCulture, out endTime))
            {
                if (!TimeSpan.TryParse(hours[1].Trim(), out endTime))
                    return slots;
            }

            // Generate one-hour slots
            while (startTime < endTime)
            {
                TimeSpan nextSlot = startTime.Add(TimeSpan.FromHours(1));
                if (nextSlot <= endTime)
                {
                    string formattedStart = $"{startTime.Hours:D2}:{startTime.Minutes:D2}";
                    string formattedEnd = $"{nextSlot.Hours:D2}:{nextSlot.Minutes:D2}";
                    slots.Add($"{formattedStart} - {formattedEnd}");
                }
                startTime = nextSlot;
            }

            return slots;
        }

        /// <summary>
        ///  FINDING AVAILABLE SLOTS FOR BOOKING
        /// </summary>
        private async Task<List<string>> GetBookedSlots(int providerId, string providerType, DateTime date)
        {
            List<string> bookedSlots = new List<string>();

            //FIND APPOINTMENT LIST BY DATE AND PROVIDER
            var appointments = await _appointmentService.GetAppointmentsByProviderAndDate(providerId, providerType, date);

            foreach (var appointment in appointments)
            {
                var appointmentStart = appointment.Date;
                var appointmentEnd = appointment.Date.AddHours(1);

                string formattedStart = $"{appointmentStart.Hour:D2}:{appointmentStart.Minute:D2}";
                string formattedEnd = $"{appointmentEnd.Hour:D2}:{appointmentEnd.Minute:D2}";

                bookedSlots.Add($"{formattedStart} - {formattedEnd}");
            }

            return bookedSlots;
        }
    };
}
