using BusinessObjects.DTOs.Facility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin
{
    public class FacilityTypeModel : PageModel
    {
        private readonly IFacilityTypeService _facilityTypeService;

        public FacilityTypeModel(IFacilityTypeService facilityTypeService)
        {
            _facilityTypeService = facilityTypeService;
        }

        public List<FacilityTypeDto> Facilities { get; set; }

        public async Task OnGetAsync()
        {
            Facilities = await _facilityTypeService.GetAllFacilityTypes();
        }

        public async Task<IActionResult> OnPostCreateAsync(FacilityTypeDto facilityType)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _facilityTypeService.Create(facilityType);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(FacilityTypeDto facilityType)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _facilityTypeService.Update(facilityType.Id, facilityType);
            return RedirectToPage();
        }
    }
}
