using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs.Appointment;
using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.DTOs.Schedule;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Patient.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly IProfessionalService _professionalService;
        private readonly IFacilityService _facilityService;
        private readonly IAppointmentService _appointmentService;

        public CreateModel(
            IProfessionalService professionalService,
            IFacilityService facilityService,
            IAppointmentService appointmentService)
        {
            _facilityService = facilityService;
            _professionalService = professionalService;
            _appointmentService = appointmentService;
        }

        /// <summary>
        /// PROPERTIES
        /// </summary>
        public List<string> TimeSlots { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public int? ProviderId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ProviderType { get; set; } = "";

        public DateOnly? ScheduleStartDate { get; set; }
        public DateOnly? ScheduleEndDate { get; set; }
        public string WorkingWeekdaysJson { get; set; } = "[]";
        public string ClosedExceptionDatesJson { get; set; } = "[]";

        [BindProperty(SupportsGet = true)]
        public DateTime? SelectedDate { get; set; } = DateTime.Today;

        public string? SelectedTimeSlot { get; set; }

        [BindProperty]
        public int SelectedServiceId { get; set; }

        public ProfessionalDto? professional { get; set; }
        public SearchingFacilityDto? facility { get; set; }
        public List<ServiceDto> services { get; set; } = new List<ServiceDto>();

        public async Task<IActionResult> OnGetAsync(
            int? ProviderId,
            string ProviderType,
            string SelectedDate,
            string SelectedTimeSlot = null,
            int SelectedServiceId = 0)
        {
            this.ProviderId = ProviderId;
            this.ProviderType = ProviderType;

            if (!string.IsNullOrEmpty(SelectedDate) && DateTime.TryParse(SelectedDate, out DateTime parsedDate))
                this.SelectedDate = parsedDate;

            this.SelectedTimeSlot = SelectedTimeSlot;
            this.SelectedServiceId = SelectedServiceId;

            TimeSlots = new List<string>();

            if (ProviderId.HasValue)
            {
                if (ProviderType == "Professional")
                {
                    professional = await _professionalService.GetById(ProviderId.Value);
                    if (professional == null) return NotFound();

                    services = professional.PrivateServices ?? new List<ServiceDto>();

                    var active = professional.ActiveSchedule
                              ?? professional.Schedules?
                                 .Where(s => s != null)
                                 .OrderBy(s => s.StartDate)
                                 .FirstOrDefault();

                    if (active != null)
                    {
                        ScheduleStartDate = active.StartDate;
                        ScheduleEndDate = active.EndDate;

                        var weekdays = (active.WorkingDates ?? new List<WorkingDateDto>())
                                        .Select(w => w.Weekday)
                                        .Distinct()
                                        .OrderBy(x => x)
                                        .ToList();

                        var closedDates = (professional.ScheduleExceptions ?? new List<ScheduleExceptionDto>())
                                            .Where(e => e.IsClosed)
                                            .Select(e => e.Date.ToString("yyyy-MM-dd"))
                                            .Distinct()
                                            .OrderBy(d => d)
                                            .ToList();

                        WorkingWeekdaysJson = JsonConvert.SerializeObject(weekdays);
                        ClosedExceptionDatesJson = JsonConvert.SerializeObject(closedDates);

                        if (!this.SelectedDate.HasValue)
                        {
                            var today = DateOnly.FromDateTime(DateTime.Today);
                            var pick = ClampToRange(today, active.StartDate, active.EndDate);
                            var valid = FindNextValidDate(pick, active, weekdays, closedDates);
                            this.SelectedDate = new DateTime(valid.Year, valid.Month, valid.Day);
                        }

                        if (this.SelectedDate.HasValue)
                        {
                            var dateOnly = DateOnly.FromDateTime(this.SelectedDate.Value.Date);

                            if (dateOnly >= active.StartDate && dateOnly <= active.EndDate)
                            {
                                int jsDow = (int)this.SelectedDate.Value.DayOfWeek; // 0..6
                                int dbDow = jsDow == 0 ? 1 : jsDow + 1;              // map sang 1..7

                                var workingDatesForDay = active.WorkingDates?
                                                            .Where(w => w.Weekday == dbDow)
                                                            .ToList();

                                var ex = professional.ScheduleExceptions?
                                                       .FirstOrDefault(e => e.Date == dateOnly);

                                if (ex?.IsClosed == true)
                                {
                                    TimeSlots.Clear(); // nghỉ hẳn
                                }
                                else if (workingDatesForDay != null && workingDatesForDay.Any())
                                {
                                    foreach (var wd in workingDatesForDay)
                                    {
                                        var start = wd.StartTime;
                                        var end = wd.EndTime;

                                        // Nếu có override exception
                                        if (ex?.NewStartTime.HasValue == true && ex?.NewEndTime.HasValue == true)
                                        {
                                            start = ex.NewStartTime.Value;
                                            end = ex.NewEndTime.Value;
                                        }

                                        string fmt(TimeOnly t) => t.ToString("H\\:mm");
                                        TimeSlots.Add($"{fmt(start)} - {fmt(end)}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this.SelectedDate ??= DateTime.Today;
                        WorkingWeekdaysJson = "[]";
                        ClosedExceptionDatesJson = "[]";
                    }
                }
                else if (ProviderType == "Facility")
                {
                    facility = await _facilityService.GetFacilityById(ProviderId.Value);
                    services = facility?.PublicServices ?? new List<ServiceDto>();

                    var start = DateOnly.FromDateTime(DateTime.Today);
                    var end = start.AddDays(30);
                    ScheduleStartDate = start;
                    ScheduleEndDate = end;

                    var weekdays = new List<int> { 2, 3, 4, 5, 6, 7 }; // T2..T7
                    WorkingWeekdaysJson = JsonConvert.SerializeObject(weekdays);
                    ClosedExceptionDatesJson = "[]";
                    this.SelectedDate ??= DateTime.Today;

                    TimeSlots.Add("7:00 - 16:00");
                }

                var bookedSlots = await GetBookedSlots(ProviderId.Value, ProviderType, this.SelectedDate!.Value);
                ViewData["BookedSlots"] = bookedSlots;
            }

            return Page();
        }

        private static DateOnly ClampToRange(DateOnly d, DateOnly start, DateOnly end)
        {
            if (d < start) return start;
            if (d > end) return end;
            return d;
        }

        private static DateOnly FindNextValidDate(
            DateOnly startTry,
            ScheduleDto active,
            List<int> weekdaysDb,
            List<string> closedDatesYmd)
        {
            var d = startTry;
            while (d <= active.EndDate)
            {
                if (d >= active.StartDate && d <= active.EndDate)
                {
                    var jsDow = (int)d.DayOfWeek; // 0..6
                    var dbDow = jsDow == 0 ? 1 : jsDow + 1;

                    var ymd = d.ToString("yyyy-MM-dd");
                    if (weekdaysDb.Contains(dbDow) && !closedDatesYmd.Contains(ymd))
                        return d;
                }
                d = d.AddDays(1);
            }
            return startTry;
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
            GeneralUserDto? currentUser = null;

            if (!string.IsNullOrEmpty(currentUserJson))
            {
                currentUser = JsonConvert.DeserializeObject<GeneralUserDto>(currentUserJson);
            }

            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập để đặt lịch.");
                return Page();
            }

            if (!DateTime.TryParse(selectedDateStr, out DateTime selectedDate))
            {
                ModelState.AddModelError(string.Empty, "Invalid date selected.");
                return Page();
            }

            // Parse "H:mm - H:mm"
            if (string.IsNullOrWhiteSpace(selectedTimeSlot))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn khung giờ.");
                return Page();
            }

            var selectedTimeSlotString = selectedTimeSlot.ToString(); var timeSlotParts = selectedTimeSlotString.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries); if (timeSlotParts.Length != 2 || !TimeSpan.TryParse(timeSlotParts[0], out TimeSpan startTime) || !TimeSpan.TryParse(timeSlotParts[1], out TimeSpan endTime)) { ModelState.AddModelError(string.Empty, "Invalid time slot format."); return Page(); }

            var appointmentDateTime = selectedDate.Date.Add(startTime);

            var createAppointmentDto = new CreateAppointmentDto
            {
                // Service sẽ tự map userId -> patient.Id
                Date = appointmentDateTime, // Service đảm bảo ExpectedStart = Date nếu ExpectedStart chưa có
                PatientId = currentUser.Id,
                ProviderId = providerId,
                ProviderType = Enum.Parse<ProviderType>(providerType),
                ServiceId = SelectedServiceId,
                Source = AppointmentSource.Booked,
                Status = AppointmentStatus.Scheduled
            };

            var result = await _appointmentService.AddAsync(createAppointmentDto);
            if (!result.IsSuccess || result.Data == null || result.Data.Id == 0)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Không thể tạo lịch hẹn.");
                return Page();
            }

            // Điều hướng sang trang hiển thị số thứ tự (Ticket)
            return RedirectToPage("/Patient/Appointment/Ticket", new { appointmentId = result.Data.Id });
        }

        private static readonly AppointmentStatus[] _effectiveStatuses = new[]
        {
            AppointmentStatus.Scheduled,
            AppointmentStatus.CheckedIn,
            AppointmentStatus.InExam,
            AppointmentStatus.Completed
        };

        private async Task<List<(DateTime start, DateTime end, int stepMinutes)>> GetWorkingIntervalsForDayAsync(
            int providerId, string providerType, DateOnly day)
        {
            var intervals = new List<(DateTime, DateTime, int)>();

            if (providerType == "Professional")
            {
                var pro = await _professionalService.GetById(providerId);
                if (pro == null) return intervals;

                var active = pro.ActiveSchedule
                          ?? pro.Schedules?.Where(s => s != null)
                                           .OrderBy(s => s.StartDate)
                                           .FirstOrDefault();
                if (active == null) return intervals;

                var jsDow = (int)day.DayOfWeek; // 0..6
                var dbDow = jsDow == 0 ? 1 : jsDow + 1;

                var wds = active.WorkingDates?.Where(w => w.Weekday == dbDow).ToList();
                if (wds == null || wds.Count == 0) return intervals;

                var ex = pro.ScheduleExceptions?.FirstOrDefault(e => e.Date == day);
                if (ex?.IsClosed == true) return intervals;

                foreach (var wd in wds)
                {
                    var start = wd.StartTime;
                    var end = wd.EndTime;

                    if (ex?.NewStartTime.HasValue == true && ex?.NewEndTime.HasValue == true)
                    {
                        start = ex.NewStartTime.Value;
                        end = ex.NewEndTime.Value;
                    }

                    if (end <= start) continue;

                    var step = wd.SlotDuration + wd.SlotBuffer;
                    var stepMinutes = (int)Math.Max(1, step.TotalMinutes);

                    var s = day.ToDateTime(start);
                    var e = day.ToDateTime(end);

                    intervals.Add((s, e, stepMinutes));
                }
            }
            else if (providerType == "Facility")
            {
                var s = day.ToDateTime(new TimeOnly(7, 0));
                var e = day.ToDateTime(new TimeOnly(16, 0));
                var stepMinutes = 30;
                intervals.Add((s, e, stepMinutes));
            }

            return intervals.OrderBy(t => t.Item1).ToList();
        }

        private async Task<List<string>> GetBookedSlots(int providerId, string providerType, DateTime date)
        {
            var result = new List<string>();
            var day = DateOnly.FromDateTime(date.Date);

            int? currentUserId = null;
            var currentUserJson = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(currentUserJson))
            {
                var currentUser = JsonConvert.DeserializeObject<GeneralUserDto>(currentUserJson);
                currentUserId = currentUser?.Id;
            }

            var intervals = await GetWorkingIntervalsForDayAsync(providerId, providerType, day);
            if (intervals.Count == 0) return result;

            var dayAppts = await _appointmentService.GetAppointmentsByProviderAndDate(providerId, providerType, date);
            var effective = dayAppts.Where(a => _effectiveStatuses.Contains(a.Status)).ToList();

            foreach (var (start, end, stepMinutes) in intervals)
            {
                var totalMinutes = (end - start).TotalMinutes;
                var slotsInBlock = Math.Max(1, (int)Math.Floor(totalMinutes / stepMinutes));

                var onlineCap = Math.Max(1, (int)Math.Floor(slotsInBlock * 0.7));

                // So sánh bằng (Date ?? ExpectedStart) để tránh null
                bool InBlock(AppointmentDTO a) =>
                    (a.Date ?? a.ExpectedStart) >= start &&
                    (a.Date ?? a.ExpectedStart) < end;

                var bookedOnlineCount = effective.Count(a =>
                    a.Source == AppointmentSource.Booked && InBlock(a));

                var currentUserHasAppt = currentUserId.HasValue &&
                    effective.Any(a => a.PatientId == currentUserId.Value && InBlock(a));

                if (bookedOnlineCount >= onlineCap || currentUserHasAppt)
                {
                    string formatted = $"{start:HH\\:mm} - {end:HH\\:mm}";
                    result.Add(formatted);
                }
            }

            return result;
        }
    };
}
