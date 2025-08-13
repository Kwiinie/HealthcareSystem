using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs.Appointment;
using BusinessObjects.Enums;
using FindingHealthcareSystem.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using BusinessObjects.Entities;

namespace FindingHealthcareSystem.Pages.Professional.Appointment
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHubContext<UpdateHub> _hubContext;
        private readonly IMemoryCache _cache;
        [BindProperty]
        public int DeleteId { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<AppointmentDTO> Appointments { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<AppointmentDTO> PatientRecently { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<AppointmentStatus> AvailableStatuses { get; set; }
        public int TotalMyAppointment { get; set; }
        public int TotalPatient { get; set; }
        public int TotalWaitAppointment { get; set; }
        public int TotalCompleteAppointment { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }
        [BindProperty(SupportsGet = true)]
        public int MaxPage { get; set; }
        public DateTime Monday { get; set; }
        public IndexModel(IAppointmentService appointmentService, IHubContext<UpdateHub> hubContext, IMemoryCache cache)
        {
            _appointmentService = appointmentService;
            _hubContext = hubContext;
            _cache = cache;
        }

        public GeneralUserDto GetUser()
        {
            var textAcc = HttpContext.Session.GetString("User");
            return JsonConvert.DeserializeObject<GeneralUserDto>(textAcc);
        }

        public async Task<IActionResult> OnGet(int pagee = 1)
        {
            try
            {
                var acc = GetUser();
                Monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                Appointments = (await _appointmentService.GetAllAppoinmentByDate(acc.Id, Monday, Monday.AddDays(7)))
                .Where(x => x.Status is AppointmentStatus.Pending or AppointmentStatus.Confirmed
                or AppointmentStatus.Completed or AppointmentStatus.Cancelled
                or AppointmentStatus.Rescheduled or AppointmentStatus.Rejected)
                .ToList();
                var result = await _appointmentService.GetPagenagingAppointments(acc.Id, pagee, 5);
                MaxPage = result.Item1;
                PatientRecently = result.Item2;
                TotalMyAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, "");
                TotalWaitAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, AppointmentStatus.Pending.ToString());
                TotalCompleteAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, AppointmentStatus.Completed.ToString());
                TotalPatient = await _appointmentService.CountTotalPatient(acc.Id);
                CurrentPage = pagee;
                //_cache.Set("Page", pagee);
                return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("/Index");
            }
        }

        public static List<AppointmentStatus> GetAvailableStatuses(AppointmentStatus currentStatus)
        {
            return currentStatus switch
            {
                AppointmentStatus.Pending => new List<AppointmentStatus> { AppointmentStatus.Pending, AppointmentStatus.Confirmed, AppointmentStatus.Rejected },
                AppointmentStatus.Rejected => new List<AppointmentStatus> { AppointmentStatus.Rejected, AppointmentStatus.Rescheduled },
                AppointmentStatus.Confirmed => new List<AppointmentStatus> { AppointmentStatus.Confirmed, AppointmentStatus.Completed },
                AppointmentStatus.Cancelled => new List<AppointmentStatus> { AppointmentStatus.Cancelled },
                AppointmentStatus.Completed => new List<AppointmentStatus> { AppointmentStatus.Completed },
                AppointmentStatus.Rescheduled => new List<AppointmentStatus> { AppointmentStatus.Rescheduled, AppointmentStatus.Completed },
                _ => new List<AppointmentStatus>()
            };
        }

        public async Task<IActionResult> OnGetNextWeek(DateTime monday, int next)
        {
            try
            {
                var acc = GetUser();
                Monday = monday.AddDays(next);
                if (next == 0)
                {
                    Monday = monday.AddDays(-(int)monday.DayOfWeek + (int)DayOfWeek.Monday);
                }

                _cache.Set("Monday", Monday);

                Appointments = (await _appointmentService.GetAllAppoinmentByDate(acc.Id, Monday, Monday.AddDays(7)))
                    .Where(x => x.Status is AppointmentStatus.Pending or AppointmentStatus.Confirmed
                    or AppointmentStatus.Completed or AppointmentStatus.Cancelled
                    or AppointmentStatus.Rescheduled or AppointmentStatus.Rejected)
                    .ToList();
                return Partial("_PatientAppointments", this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new StatusCodeResult(500);
            }
        }


        public async Task fetchDataSignalR()
        {
            var acc = GetUser();
            TotalMyAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, "");
            TotalWaitAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, AppointmentStatus.Pending.ToString());
            TotalCompleteAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, AppointmentStatus.Completed.ToString());
            await _hubContext.Clients.All.SendAsync("UpdateProfessionalAppointmentInfo", new
            {
                totalMyAppointment = TotalMyAppointment,
                totalCompleteAppointment = TotalCompleteAppointment,
                totalWaitAppointment = TotalWaitAppointment
            });
        }

        public async Task<IActionResult> OnGetChangeAppointmentStatus(int id, int status, DateTime date, string slot = "", string entity = null, string diagnose = "")
        {
            try
            {
                if (status == (int)AppointmentStatus.Rescheduled)
                {
                    await _appointmentService.ChangeAppointmentStatus(id, AppointmentStatus.Cancelled);
                    RescheduleAppointmentDTO? reschedule = JsonConvert.DeserializeObject<RescheduleAppointmentDTO>(entity);
                    if (reschedule != null)
                    {
                        reschedule.Status = (AppointmentStatus)status;
                        reschedule.Date = date.Add(TimeSpan.Parse(slot));
                        var result = await _appointmentService.AddAsync(reschedule);
                    }
                }
                else if (status == (int)AppointmentStatus.Completed)
                {
                    await _appointmentService.ChangeAppointmentStatus(id, (AppointmentStatus)status);
                    await _appointmentService.UpdateAppointmentDiagnose(id, diagnose);
                }
                else
                {
                    bool success = await _appointmentService.ChangeAppointmentStatus(id, (AppointmentStatus)status);
                }
                var acc = GetUser();
                Monday = _cache.Get<DateTime>("Monday");
                Appointments = (await _appointmentService.GetAllAppoinmentByDate(acc.Id, Monday, Monday.AddDays(7)))
                .Where(x => x.Status is AppointmentStatus.Pending or AppointmentStatus.Confirmed
                or AppointmentStatus.Completed or AppointmentStatus.Cancelled
                or AppointmentStatus.Rescheduled or AppointmentStatus.Rejected)
                .ToList();
                await fetchDataSignalR();
                return Partial("_PatientAppointments", this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> OnGetAppointment(int id, ServiceType type)
        {
            AppointmentDTO appointment = await _appointmentService.GetAppointmentByDateAndSlot(id, type);
            AvailableStatuses = GetAvailableStatuses(appointment.Status);
            return new JsonResult(new
            {
                appointment,
                AvailableStatuses
            });
        }

        public async Task<IActionResult> OnGetLoadPage(int pagee)
        {
            var acc = GetUser();
            var result = await _appointmentService.GetPagenagingAppointments(acc.Id, pagee, 5);
            MaxPage = result.Item1;
            PatientRecently = result.Item2;
            CurrentPage = pagee;
            return Partial("_PatientRecords", this);
        }


        public async Task<IActionResult> OnGetSlotsInDay(DateTime date, string slots)
        {
            List<string> list = await _appointmentService.GetSlotsExistedByDate(date, [.. slots.Split(',')]);
            return new JsonResult(new
            {
                existedSlots = list
            });
        }
    }
}
