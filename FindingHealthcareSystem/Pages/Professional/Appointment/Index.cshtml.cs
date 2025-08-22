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
using System.Linq;

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
        public int TotalWaitAppointment { get; set; }          // “chờ” = Scheduled
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

                // Tính thứ Hai của tuần hiện tại
                Monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

                // Lấy lịch hẹn trong tuần & lọc theo enum mới
                Appointments = (await _appointmentService.GetAllAppoinmentByDate(acc.Id, Monday, Monday.AddDays(7)))
                    .Where(x => x.Status is AppointmentStatus.Scheduled
                                         or AppointmentStatus.CheckedIn
                                         or AppointmentStatus.InExam
                                         or AppointmentStatus.Completed
                                         or AppointmentStatus.CancelledByPatient
                                         or AppointmentStatus.CancelledByDoctor
                                         or AppointmentStatus.NoShow)
                    .ToList();

                var result = await _appointmentService.GetPagenagingAppointments(acc.Id, pagee, 5);
                MaxPage = result.Item1;
                PatientRecently = result.Item2;

                // Đếm theo enum mới
                TotalMyAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, "");
                TotalWaitAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, AppointmentStatus.Scheduled.ToString());
                TotalCompleteAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, AppointmentStatus.Completed.ToString());
                TotalPatient = await _appointmentService.CountTotalPatient(acc.Id);

                CurrentPage = pagee;
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index");
            }
        }

        // Trạng thái khả dụng dựa theo enum mới
        public static List<AppointmentStatus> GetAvailableStatuses(AppointmentStatus currentStatus)
        {
            return currentStatus switch
            {
                // Thường front-desk sẽ chuyển Scheduled -> CheckedIn khi BN đến
                AppointmentStatus.Scheduled => new()
                {
                    AppointmentStatus.Scheduled,
                    AppointmentStatus.CheckedIn,
                    AppointmentStatus.CancelledByDoctor  // bác sĩ hủy lịch
                },

                // BS bắt đầu khám
                AppointmentStatus.CheckedIn => new()
                {
                    AppointmentStatus.CheckedIn,
                    AppointmentStatus.InExam,
                    AppointmentStatus.CancelledByDoctor
                },

                AppointmentStatus.InExam => new()
                {
                    AppointmentStatus.InExam,
                    AppointmentStatus.Completed,
                    AppointmentStatus.CancelledByDoctor
                },

                AppointmentStatus.Completed => new() { AppointmentStatus.Completed },

                AppointmentStatus.CancelledByPatient => new() { AppointmentStatus.CancelledByPatient },
                AppointmentStatus.CancelledByDoctor => new() { AppointmentStatus.CancelledByDoctor },
                AppointmentStatus.NoShow => new() { AppointmentStatus.NoShow },

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
                    .Where(x => x.Status is AppointmentStatus.Scheduled
                                         or AppointmentStatus.CheckedIn
                                         or AppointmentStatus.InExam
                                         or AppointmentStatus.Completed
                                         or AppointmentStatus.CancelledByPatient
                                         or AppointmentStatus.CancelledByDoctor
                                         or AppointmentStatus.NoShow)
                    .ToList();

                return Partial("_PatientAppointments", this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new StatusCodeResult(500);
            }
        }

        public async Task fetchDataSignalR()
        {
            var acc = GetUser();
            TotalMyAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, "");
            TotalWaitAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, AppointmentStatus.Scheduled.ToString());
            TotalCompleteAppointment = await _appointmentService.CountAppointmentByStatus(acc.Id, AppointmentStatus.Completed.ToString());

            await _hubContext.Clients.All.SendAsync("UpdateProfessionalAppointmentInfo", new
            {
                totalMyAppointment = TotalMyAppointment,
                totalCompleteAppointment = TotalCompleteAppointment,
                totalWaitAppointment = TotalWaitAppointment
            });
        }

        /// <summary>
        /// Đổi trạng thái hoặc đổi lịch:
        /// - Nếu truyền slot + entity (RescheduleAppointmentDTO) => hủy lịch cũ (CancelledByDoctor) và tạo lịch mới (Scheduled).
        /// - Nếu Completed => cập nhật chẩn đoán.
        /// - Ngược lại => đổi trạng thái theo enum mới.
        /// </summary>
        public async Task<IActionResult> OnGetChangeAppointmentStatus(
            int id,
            int status,
            DateTime date,
            string slot = "",
            string entity = null,
            string diagnose = "")
        {
            try
            {
                // Nhánh ĐỔI LỊCH (không còn enum Rescheduled): dựa vào slot + entity
                if (!string.IsNullOrWhiteSpace(slot) && !string.IsNullOrWhiteSpace(entity))
                {
                    // 1) Hủy lịch cũ do bác sĩ
                    await _appointmentService.ChangeAppointmentStatus(id, AppointmentStatus.CancelledByDoctor);

                    // 2) Tạo lịch mới ở trạng thái Scheduled
                    var reschedule = JsonConvert.DeserializeObject<RescheduleAppointmentDTO>(entity);
                    if (reschedule != null)
                    {
                        reschedule.Status = AppointmentStatus.Scheduled;
                        // ExpectedStart = date + slot
                        reschedule.Date = date.Add(TimeSpan.Parse(slot));
                        var _ = await _appointmentService.AddAsync(reschedule);
                    }
                }
                else
                {
                    var newStatus = (AppointmentStatus)status;

                    if (newStatus == AppointmentStatus.Completed)
                    {
                        await _appointmentService.ChangeAppointmentStatus(id, newStatus);
                        if (!string.IsNullOrWhiteSpace(diagnose))
                        {
                            await _appointmentService.UpdateAppointmentDiagnose(id, diagnose);
                        }
                    }
                    else
                    {
                        await _appointmentService.ChangeAppointmentStatus(id, newStatus);
                    }
                }

                // Reload danh sách tuần hiện tại
                var acc = GetUser();
                Monday = _cache.TryGetValue("Monday", out DateTime savedMonday) ? savedMonday
                                                                                : DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

                Appointments = (await _appointmentService.GetAllAppoinmentByDate(acc.Id, Monday, Monday.AddDays(7)))
                    .Where(x => x.Status is AppointmentStatus.Scheduled
                                         or AppointmentStatus.CheckedIn
                                         or AppointmentStatus.InExam
                                         or AppointmentStatus.Completed
                                         or AppointmentStatus.CancelledByPatient
                                         or AppointmentStatus.CancelledByDoctor
                                         or AppointmentStatus.NoShow)
                    .ToList();

                await fetchDataSignalR();
                return Partial("_PatientAppointments", this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> OnGetAppointment(int id, ServiceType type)
        {
            var appointment = await _appointmentService.GetAppointmentByDateAndSlot(id, type);
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
