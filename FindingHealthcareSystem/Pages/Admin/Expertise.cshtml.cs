using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin
{
    public class ExpertiseModel : PageModel
    {
        private readonly IExpertiseService _expertiseService;          

        public ExpertiseModel(IExpertiseService expertiseService)
        {
            _expertiseService = expertiseService;
        }

        [BindProperty]
        public ExpertiseDTO Expertise { get; set; }

        public List<ExpertiseDTO> Expertises { get; set; } = new List<ExpertiseDTO>();

        public async Task OnGetAsync()
        {
            Expertises = await _expertiseService.GetAllExpertises();
        }

        public async Task<IActionResult> OnPostCreateAsync(ExpertiseDTO expertise)
        {
            if (!ModelState.IsValid)
            {
                Expertises = await _expertiseService.GetAllExpertises();
                return Page();
            }

            // Đảm bảo các giá trị mặc định
            expertise.IsDeleted = expertise.IsDeleted ?? false;
            expertise.CreatedAt = DateTime.Now;
            expertise.UpdatedAt = DateTime.Now;

            await _expertiseService.CreateExpertise(expertise);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(ExpertiseDTO expertise)
        {
            if (!ModelState.IsValid)
            {
                Expertises = await _expertiseService.GetAllExpertises();
                return Page();
            }

            if (expertise.Id == null)
            {
                return NotFound();
            }

            // Đảm bảo giá trị cập nhật
            expertise.UpdatedAt = DateTime.Now;

            await _expertiseService.UpdateExpertise(expertise.Id.Value, expertise);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _expertiseService.DeleteExpertise(id);
            return RedirectToPage();
        }
    }
}
