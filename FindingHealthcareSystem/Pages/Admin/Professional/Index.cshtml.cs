using BusinessObjects.Commons;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Services.Services;

namespace FindingHealthcareSystem.Pages.Admin.Professional
{
    public class IndexModel : PageModel
    {
        private readonly IProfessionalService _professionalService;
        private readonly IUserService _userService;

        public IndexModel(IProfessionalService professionalService, IUserService userService)
        {
            _professionalService = professionalService;
            _userService = userService;
        }

        public IEnumerable<ProfessionalDto> Professionals { get; set; }
        public IEnumerable<ProfessionalDto> PendingProfessionals { get; set; }
        public ProfessionalDto Professional { get; set; } = new();

        public PaginatedList<ProfessionalDto> ProfessionalsPaged { get; set; }

        public async Task OnGetAsync()
        {
            Professionals = await _professionalService.GetAllProfessionalAsync(ProfessionalRequestStatus.Approved);
            PendingProfessionals = await _professionalService.GetAllProfessionalAsync(ProfessionalRequestStatus.Pending);
            //ProfessionalsPaged = await PaginatedList<ProfessionalDto>.CreateAsync(Professionals.AsQueryable(), 1, 10);
        }

        public async Task<IActionResult> OnPostApproveAsync(int professionalId)
        {
            return await ProcessRequest(professionalId, ProfessionalRequestStatus.Approved);
        }

        public async Task<IActionResult> OnPostRejectAsync(int professionalId)
        {
            return await ProcessRequest(professionalId, ProfessionalRequestStatus.Rejected);
        }


        private async Task<IActionResult> ProcessRequest(int professionalId, ProfessionalRequestStatus status)
        {
            if (professionalId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Dữ liệu không hợp lệ.");
                return Page();
            }

            try
            {
                var professional = await _professionalService.GetProfessionalByProId(professionalId);
                if (professional == null)
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy chuyên gia.");
                    return Page();
                }

                professional.RequestStatus = status;
                var user = await _userService.GetUserByIdAsync(professional.UserId.GetValueOrDefault());
                user.Status= UserStatus.Inactive.ToString(); 

                await _userService.UpdateUserStatus(user);

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi xử lý yêu cầu: {ex.Message}");
                return Page();
            }
        }
    }
}
