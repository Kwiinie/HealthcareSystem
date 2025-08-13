using BusinessObjects.DTOs.Professional;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin
{
    public class SpecialtyModel : PageModel
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtyModel(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        public List<SpecialtyDto> Specialties { get; set; } = new List<SpecialtyDto>();
        public List<SpecialtyDto> AllSpecialties { get; set; } = new List<SpecialtyDto>();

        [BindProperty]
        public SpecialtyDto Specialty { get; set; } = new SpecialtyDto();

        [BindProperty]
        public string SearchTerm { get; set; }

        [BindProperty]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        public DateTime? EndDate { get; set; }

        [BindProperty]
        public string DateType { get; set; } = "Created"; // Default to searching by creation date

        public async Task OnGetAsync()
        {
            // Load all specialties once
            AllSpecialties = await _specialtyService.GetAllSpecialties();
            Specialties = AllSpecialties;
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                await _specialtyService.CreateSpecialty(Specialty);
                TempData["SuccessMessage"] = "Chuyên khoa đã được tạo thành công.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo chuyên khoa: " + ex.Message);
                AllSpecialties = await _specialtyService.GetAllSpecialties();
                Specialties = AllSpecialties;
                return Page();
            }
        }


        public async Task<IActionResult> OnPostEditAsync()
        {
            try
            {
                await _specialtyService.UpdateSpecialty(Specialty);
                TempData["SuccessMessage"] = "Chuyên khoa đã được cập nhật thành công.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật chuyên khoa: " + ex.Message);
                AllSpecialties = await _specialtyService.GetAllSpecialties();
                Specialties = AllSpecialties;
                return Page();
            }
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _specialtyService.DeleteSpecialty(id);
                TempData["SuccessMessage"] = "Chuyên khoa đã được xóa thành công.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi xóa chuyên khoa: " + ex.Message);
                AllSpecialties = await _specialtyService.GetAllSpecialties();
                Specialties = AllSpecialties;
                return Page();
            }
        }


        public async Task<IActionResult> OnPostSearchAsync()
        {
            // Get all specialties first to ensure we have current data
            AllSpecialties = await _specialtyService.GetAllSpecialties();

            // Start with all specialties and apply filters
            var filteredSpecialties = AllSpecialties;

            // Apply name filter if provided
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                filteredSpecialties = filteredSpecialties
                    .Where(s => s.Name?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }

            // Apply date filters if provided
            if (StartDate.HasValue)
            {
                DateTime startDateValue = StartDate.Value.Date;
                if (DateType == "Created")
                {
                    filteredSpecialties = filteredSpecialties
                        .Where(s => s.CreatedAt?.Date >= startDateValue)
                        .ToList();
                }
                else // Updated
                {
                    filteredSpecialties = filteredSpecialties
                        .Where(s => s.UpdatedAt?.Date >= startDateValue)
                        .ToList();
                }
            }

            if (EndDate.HasValue)
            {
                DateTime endDateValue = EndDate.Value.Date.AddDays(1).AddTicks(-1); // End of the day
                if (DateType == "Created")
                {
                    filteredSpecialties = filteredSpecialties
                        .Where(s => s.CreatedAt?.Date <= endDateValue.Date)
                        .ToList();
                }
                else // Updated
                {
                    filteredSpecialties = filteredSpecialties
                        .Where(s => s.UpdatedAt?.Date <= endDateValue.Date)
                        .ToList();
                }
            }

            // Update the Specialties list with filtered results
            Specialties = filteredSpecialties;

            return Page();
        }

        public async Task<IActionResult> OnPostResetAsync()
        {
            // Reset all filters and reload all specialties
            AllSpecialties = await _specialtyService.GetAllSpecialties();
            Specialties = AllSpecialties;

            // Clear search properties
            SearchTerm = null;
            StartDate = null;
            EndDate = null;
            DateType = "Created";

            return Page();
        }
    }
}
