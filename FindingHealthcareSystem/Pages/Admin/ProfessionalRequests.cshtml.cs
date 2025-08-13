using BusinessObjects.DTOs.Professional;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindingHealthcareSystem.Pages.Admin
{
    public class ProfessionalRequestsModel : PageModel
    {
        private readonly IProfessionalService _professionalService;
        private readonly IUserService _userService;

        public ProfessionalRequestsModel(IProfessionalService professionalService, IUserService userService)
        {
            _professionalService = professionalService;
            _userService = userService;
        }

        public List<ProfessionalDto> PendingProfessionals { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                var professionals = await _professionalService.GetAllProfessionalAsync(ProfessionalRequestStatus.Pending);
                PendingProfessionals = professionals.ToList();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi tải dữ liệu: {ex.Message}");
            }
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

                await _userService.UpdateProfessionalAsync(professional);

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
