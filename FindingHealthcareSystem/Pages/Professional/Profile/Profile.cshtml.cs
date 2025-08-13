using BusinessObjects.DTOs.Professional;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Entities;
using FindingHealthcareSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Professional.Profile
{
    public class ProfileModel : PageModel
    {
        private readonly IProfessionalService _professionalService;
        private readonly IUserService _userService;
        private readonly IFileUploadService _fileUploadService;

        public ProfileModel(IProfessionalService professionalService, IUserService userService, IFileUploadService fileUploadService)
        {
            _professionalService = professionalService;
            _userService = userService;
            _fileUploadService = fileUploadService;
        }
        public BusinessObjects.Entities.Professional CurrentUser { get; set; } // Chứa thông tin
                                                                               // user
        [BindProperty]
        public BusinessObjects.Entities.Professional UpdatedUser { get; set; } // Thuộc tính để binding dữ liệu từ form

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        public ProfessionalDto professional { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public List<ServiceDto> Services { get; set; }
        [BindProperty]
        public ServiceDto Service { get; set; }


        [BindProperty(SupportsGet = true)]
        public int FacilityId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ServiceId { get; set; }
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
            FacilityId = userId;



            // Truy vấn thông tin User từ Database
            var user = await _userService.GetUserByIdNew(userId);
            if (user == null)
            {
                return RedirectToPage("/Login"); // Nếu User không tồn tại, quay về Login
            }

            // Truy vấn thông tin Professional từ Database
            CurrentUser = await _userService.GetProfessionalById(userId);
            if (CurrentUser == null)
            {
                return RedirectToPage("/Login"); // Nếu Professional không tồn tại, quay về Login
            }

            var professional = await _userService.GetProfessionalById(userId);
            if (professional == null) return NotFound();
            Services = await _professionalService.GetServicesByProId(professional.Id) ?? new List<ServiceDto>();

            // Khởi tạo UpdatedUser với thông tin từ cả hai bảng
            UpdatedUser = new BusinessObjects.Entities.Professional
            {
                UserId = user.Id,
                Province = CurrentUser.Province,
                District = CurrentUser.District,
                Ward = CurrentUser.Ward,
                Address = CurrentUser.Address,
                Degree = CurrentUser.Degree,
                Experience = CurrentUser.Experience,
                WorkingHours = CurrentUser.WorkingHours,
                ProfessionalSpecialties = CurrentUser.ProfessionalSpecialties,
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

            return Page();
        }

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

                var professional = await _userService.GetProfessionalById(userId);
                if (professional == null) return NotFound();

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
                professional.Province = UpdatedUser.Province;
                professional.District = UpdatedUser.District;
                professional.Ward = UpdatedUser.Ward;
                professional.Address = UpdatedUser.Address;
                professional.Degree = UpdatedUser.Degree;
                professional.Experience = UpdatedUser.Experience;
                professional.WorkingHours = UpdatedUser.WorkingHours;
                professional.ProfessionalSpecialties = UpdatedUser.ProfessionalSpecialties;
                await _userService.UpdateUserAsync(user);
                await _userService.UpdateProfessionalAsync(professional);
                TempData["SuccessMessage"] = "Thay đồi hồ sơ thành công! ";
                await OnGet();
                return Page();
            }
            catch (Exception ex)
            {
                await OnGet();
                TempData["ErrorMessage"] = ex.Message + ", Thay đồi hồ sơ thành công!";
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAddService()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
            {
                return Unauthorized();
            }
            var userObject = JsonConvert.DeserializeObject<dynamic>(userJson);
            int userId = userObject.Id;
            var professional = await _userService.GetProfessionalById(userId);
            if (professional == null) return NotFound();
            var Name = Service.Name;
            var Price = Service.Price;
            var Description = Service.Description;

            await _professionalService.Create(professional.Id, Service);

            return RedirectToPage(); // Hoặc thay bằng trang bạn muốn quay về
        }

        // Cập nhật dịch vụ
        public async Task<IActionResult> OnPostEditServiceAsync()
        {


            var existingService = await _professionalService.GetPrivateServiceById(ServiceId);
            if (existingService == null)
            {
                return NotFound();
            }
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
            {
                return Unauthorized();
            }
            var userObject = JsonConvert.DeserializeObject<dynamic>(userJson);
            int userId = userObject.Id;
            var professional = await _userService.GetProfessionalById(userId);
            if (professional == null) return NotFound();

            existingService.Name = Service.Name;
            existingService.Price = Service.Price;
            existingService.Description = Service.Description;

            await _professionalService.Update(professional.Id, ServiceId, existingService);
            return RedirectToPage(); // Hoặc thay bằng trang bạn muốn quay về
        }

        // Xóa dịch vụ
        public async Task<IActionResult> OnPostDeleteServiceAsync()
        {
            var existingService = await _professionalService.GetPrivateServiceById(ServiceId);
            if (existingService == null)
            {
                return NotFound();
            }

            await _professionalService.Delete(ServiceId);

            return RedirectToPage(); // Hoặc thay bằng trang bạn muốn quay về
        }
    }
}
