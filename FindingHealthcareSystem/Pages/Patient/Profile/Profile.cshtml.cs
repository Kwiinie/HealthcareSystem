using BusinessObjects.DTOs.Appointment;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.Services;

namespace FindingHealthcareSystem.Pages.Patient.Profile
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService; // Inject Repository
        private readonly IAppointmentService _appointmentService;
        private readonly IFileUploadService _fileUploadService;


        public BusinessObjects.Entities.Patient CurrentUser { get; set; } // Chứa thông tin
                                                                               // user
        [BindProperty]
        public BusinessObjects.Entities.Patient UpdatedUser { get; set; } // Thuộc tính để binding dữ liệu từ form
        public List<MyAppointmentDto> Appointments { get; set; } = new();

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        public ProfileModel(IUserService userService, IAppointmentService appointmentService, IFileUploadService fileUploadService)
        {
            _userService = userService;
            _appointmentService = appointmentService;
            _fileUploadService = fileUploadService;
        }
        public async Task<IActionResult> OnGet()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToPage("/Login"); // Chưa đăng nhập -> Quay về Login
            }

            // Chỉ lấy ID từ JSON
            var userObject = JsonConvert.DeserializeObject<dynamic>(userJson);
            int userId = userObject.Id; // Lấy UserId từ Session

            // Truy vấn thông tin User từ Database
            var user = await _userService.GetUserByIdNew(userId);
            if (user == null)
            {
                return RedirectToPage("/Login"); // Nếu User không tồn tại, quay về Login
            }

            // Truy vấn thông tin Professional từ Database
            CurrentUser = await _userService.GetPatientById(userId);
            if (CurrentUser == null)
            {
                return RedirectToPage("/Login"); // Nếu Professional không tồn tại, quay về Login
            }

            // Khởi tạo UpdatedUser với thông tin từ cả hai bảng
            UpdatedUser = new BusinessObjects.Entities.Patient
            {
                UserId = user.Id,
                Note = CurrentUser.Note,
               
                User = new User
                {
                    Fullname = user.Fullname,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    Birthday = user.Birthday,
                    ImgUrl = user.ImgUrl
                }
            };

            var allAppointments = await _appointmentService.GetMyAppointment(userId);
            var filtered = allAppointments.AsQueryable();
            Appointments = filtered
                .Where(a => a.Date.Date >= DateTime.UtcNow.Date &&
                           (a.Status == AppointmentStatus.Pending ||
                            a.Status == AppointmentStatus.Confirmed ||
                            a.Status == AppointmentStatus.Rescheduled))
                .ToList();

            return Page();
        }

        // Xử lý cập nhật hồ sơ
        public async Task<IActionResult> OnPostEditProfile()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var userJson = HttpContext.Session.GetString("User");
                if (string.IsNullOrEmpty(userJson))
                {
                    return Unauthorized();
                }

                var userObject = JsonConvert.DeserializeObject<dynamic>(userJson);
                int userId = userObject.Id;

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) return NotFound();

                var patient = await _userService.GetPatientById(userId);
                if (patient == null) return NotFound();

                // Handle image upload if a new image was provided
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    // Use FileUploadService to upload the image
                    string imageUrl = await _fileUploadService.UploadImageAsync(ProfileImage, "users");
                    await _userService.UploadUserImageAsync(userId, imageUrl);
                    user.ImgUrl = imageUrl;

                }

                // Cập nhật thông tin User
                user.Fullname = UpdatedUser.User.Fullname;
                user.Email = UpdatedUser.User.Email;
                user.PhoneNumber = UpdatedUser.User.PhoneNumber;
                user.Gender = UpdatedUser.User.Gender;
                user.Birthday = UpdatedUser.User.Birthday.GetValueOrDefault();
                

                // Cập nhật thông tin Professional
                patient.Note = UpdatedUser.Note;

                await _userService.UpdateUserAsync(user);
                await _userService.UpdatePatientAsync(patient);
                TempData["SuccessMessage"] = "Thay đồi hồ sơ thành công! ";

                await OnGet(); // Gọi lại OnGet() để reload thông tin mới
                return Page();
            }
            catch (Exception ex)
            {
                await OnGet();
                TempData["ErrorMessage"] = ex.Message + ", Thay đồi hồ sơ thành công!";
                return Page();
            }



        }

    }

}
