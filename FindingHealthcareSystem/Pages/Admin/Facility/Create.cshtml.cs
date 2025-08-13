using BusinessObjects.DTOs.Facility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Facility
{
    public class CreateModel : PageModel
    {
        private readonly IFacilityService _facilityService;
        private readonly IFacilityTypeService _facilityTypeService;

        public CreateModel(IFacilityService facilityService, IFacilityTypeService facilityTypeService)
        {
            _facilityService = facilityService;
            _facilityTypeService = facilityTypeService;
        }

        [BindProperty]
        public FacilityDto Facility { get; set; }

        public List<FacilityTypeDto> FacilityTypes { get; set; }

        public async Task OnGetAsync()
        {
            FacilityTypes = await _facilityTypeService.GetAllFacilityTypes();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                FacilityTypes = await _facilityTypeService.GetAllFacilityTypes();
                return Page();
            }

            await _facilityService.Create(Facility);
            return RedirectToPage("Index");
        }
    }
}
